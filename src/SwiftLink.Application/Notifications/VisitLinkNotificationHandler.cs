using MediatR;
using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.Notifications;

public class VisitLinkNotification : INotification
{
    public int LinkId { get; set; }
    public string ClientMetaData { get; set; }
}

public class VisitLinkNotificationHandler(IApplicationDbContext dbContext) : INotificationHandler<VisitLinkNotification>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task Handle(VisitLinkNotification notification, CancellationToken cancellationToken)
    {
        _dbContext.Set<LinkVisit>().Add(new LinkVisit
        {
            LinkId = notification.LinkId,
            ClientMetaData = notification.ClientMetaData,
            Date = DateTime.Now
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
