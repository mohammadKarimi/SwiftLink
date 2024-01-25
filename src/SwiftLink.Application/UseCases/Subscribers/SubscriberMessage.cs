namespace SwiftLink.Application.UseCases.Subscribers;

internal record SubscriberMessage
{
    public static readonly Error SubscriberMustBeSent = Error.Validation("TokenNotSended", "Token must be sent!");
    public static readonly Error InvalidToken = Error.NotFound("UnAuthorized", "Invalid User!");
}