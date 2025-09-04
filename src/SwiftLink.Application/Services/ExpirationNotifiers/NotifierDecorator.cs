using SwiftLink.Application.Notifications;

namespace SwiftLink.Application.Services.ExpirationNotifiers;
public abstract class NotifierDecorator : IExpirationNotifierComponent
{
    protected readonly IExpirationNotifierComponent notifier;

    public NotifierDecorator(IExpirationNotifierComponent notifier)
    {
        this.notifier = notifier;
    }

    public virtual async Task SendAsync(LinkExpirationNotification notification, CancellationToken cancellationToken)
    {
        await notifier.SendAsync(notification, cancellationToken);
    }
}
