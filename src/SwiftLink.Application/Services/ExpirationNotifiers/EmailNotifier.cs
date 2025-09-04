using SwiftLink.Application.Notifications;

namespace SwiftLink.Application.Services.ExpirationNotifiers;
public class EmailNotifier : IExpirationNotifierComponent
{
    public async Task SendAsync(LinkExpirationNotification notification, CancellationToken cancellationToken)
    {
        //await emailservice.send()
    }
}
