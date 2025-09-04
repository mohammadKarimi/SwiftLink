using SwiftLink.Application.Services.ExpirationNotifiers;

namespace SwiftLink.Application.Notifications;
public record LinkExpirationNotification : INotification
{
    public int LinkId { get; init; }
    public string SubscriberEmail { get; init; }
    public DateTime ExpirationDate { get; init; }
}

public class LinkExpirationNotificationHandler : INotificationHandler<LinkExpirationNotification>
{
    private readonly IExpirationNotifierComponent notifier;

    public LinkExpirationNotificationHandler(IExpirationNotifierComponent notifier)
    {
        this.notifier = notifier;
    }

    public async Task Handle(LinkExpirationNotification notification, CancellationToken cancellationToken)
    {
        await notifier.SendAsync(notification, cancellationToken);
    }
}
