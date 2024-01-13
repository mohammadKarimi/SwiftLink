namespace SwiftLink.Application.UseCases.Subscribers;
internal record SubscriberMessage
{
    public static Error SubscriberMustBeSent = Error.Validation("TokenNotSended", "Token must be sent!");

}
