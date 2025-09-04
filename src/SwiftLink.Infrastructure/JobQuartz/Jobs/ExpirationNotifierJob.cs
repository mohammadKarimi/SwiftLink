using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using SwiftLink.Application.Notifications;
using SwiftLink.Application.Services.ExpirationNotifiers;

namespace SwiftLink.Infrastructure.JobQuartz.Jobs;

//If a job takes a long time, the next trigger can create another instance of the job.
//That's why we use [DisallowConcurrentExecution].
[DisallowConcurrentExecution]
public class ExpirationNotifierJob(IApplicationDbContext applicationDbContext,
    IExpirationNotifierComponent notificationDecorator,
    IMediator mediator,
    ILogger<ExpirationNotifierJob> logger) : IJob
{
    private readonly IApplicationDbContext appDbContext = applicationDbContext;
    private readonly IExpirationNotifierComponent notifier = notificationDecorator;
    private readonly IMediator mediator = mediator;
    private readonly ILogger<ExpirationNotifierJob> logger = logger;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Starting job {JobName} at {Date}", context.JobDetail.Key.Name, DateTime.Now);
        try
        {
            var currentDate = DateTime.Now;
            var maxDate = currentDate.AddDays(1);

            var links = await appDbContext.Set<Link>()
                .Include(p => p.Subscriber)
                .Where
                        (p => !p.IsDisabled &&
                            !p.ExpirationNotificationSent &&
                             p.ExpirationDate >= currentDate &&
                             p.ExpirationDate <= maxDate
                        )
                .ToListAsync(context.CancellationToken);

            if (links.Any())
            {
                logger.LogInformation("Found {Count} expiring links to notify.", links.Count);

                var tasks = links.Select(async link =>
                {
                    await mediator.Publish(new LinkExpirationNotification
                    {
                        ExpirationDate = link.ExpirationDate,
                        LinkId = link.Id,
                        SubscriberEmail = link.Subscriber.Email
                    }, context.CancellationToken);
                    link.MarkNotificationAsSent();
                });

                await Task.WhenAll(tasks);

                /**sequencial Mode Processing/
                //foreach (var link in links)
                //{
                //    await mediator.Publish(new LinkExpirationNotification
                //    {
                //        ExpirationDate = link.ExpirationDate,
                //        LinkId = link.Id,
                //        SubscriberEmail = link.Subscriber.Email
                //    }, context.CancellationToken);

                //    link.MarkNotificationAsSent();
                //}
                /**/

                await appDbContext.SaveChangesAsync(context.CancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during job execution. Job: {JobName}", context.JobDetail.Key.Name);
        }

        logger.LogInformation("job {JobName} Finished at {Date}", context.JobDetail.Key.Name, DateTime.Now);
    }
}
