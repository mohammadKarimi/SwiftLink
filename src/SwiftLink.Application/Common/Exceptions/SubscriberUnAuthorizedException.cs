namespace SwiftLink.Application.Common.Exceptions;
public class SubscriberUnAuthorizedException : Exception
{
    public SubscriberUnAuthorizedException() : base("Token is not Valid! :(")
    {
    }
}
