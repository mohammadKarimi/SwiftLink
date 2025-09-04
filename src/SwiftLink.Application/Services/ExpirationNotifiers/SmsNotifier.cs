using SwiftLink.Application.Notifications;

namespace SwiftLink.Application.Services.ExpirationNotifiers;
public class SmsNotifier(IExpirationNotifierComponent notifier) : NotifierDecorator(notifier)
{
    public override async Task SendAsync(LinkExpirationNotification notification, CancellationToken cancellationToken)
    {
        //await smsservice.send
        await base.SendAsync(notification, cancellationToken);
    }
}
