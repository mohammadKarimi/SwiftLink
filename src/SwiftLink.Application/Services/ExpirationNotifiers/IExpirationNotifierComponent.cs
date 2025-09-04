using SwiftLink.Application.Notifications;

namespace SwiftLink.Application.Services.ExpirationNotifiers;
public interface IExpirationNotifierComponent
{
    Task SendAsync(LinkExpirationNotification notification, CancellationToken cancellationToken);
}
