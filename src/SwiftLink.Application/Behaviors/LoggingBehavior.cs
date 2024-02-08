using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.Behaviors;

public class LoggingBehavior<TRequest>(ILogger<TRequest> logger, IUser user, IApplicationDbContext dbContext) 
: IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger = logger;
    private readonly IUser _user = user;
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var subscriberToken = _user.Token.ToString() ?? string.Empty;
        string subscriberName = string.Empty;

        if (!string.IsNullOrEmpty(subscriberToken))
        {
            var result = await _dbContext.Set<Subscriber>()
                .FirstOrDefaultAsync(x => x.Token == _user.Token && x.IsActive, cancellationToken);

            subscriberName = result.Name;
        }

        _logger.LogInformation("SwiftLink Request: {Name} {@subscriberToken} {@subscriberName} {@Request}",
            requestName, subscriberToken, subscriberName, request);
    }
}
