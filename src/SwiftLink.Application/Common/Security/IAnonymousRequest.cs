namespace SwiftLink.Application.Common.Security;

/// <summary>
/// This interface marker is aimed to detect wich command and query must not go through the SubscriberAuthorizationBehavior.
/// </summary>
public interface IAnonymousRequest
{
}
