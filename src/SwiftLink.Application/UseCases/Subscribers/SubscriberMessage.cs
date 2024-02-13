namespace SwiftLink.Application.UseCases.Subscribers;

internal record SubscriberMessage
{
    public static readonly Error SubscriberMustBeSent = Error.Validation("TokenNotSended", "Token must be sent!");
    public static readonly Error InvalidToken = Error.NotFound("UnAuthorized", "Invalid User!");

    public static readonly Error NameMustBeSent = Error.Validation("Validation", "Name must be sent.");
    public static readonly Error EmailMustBeSent = Error.Validation("Validation", "Email must be sent.");
    public static readonly Error EmailFormatIsIncorrect = Error.Validation("Validation", "Email must be in correct format.");
    public static readonly Error EmailMustBeUnique = Error.Validation("Validation", "Email is exist in our system.");
}
