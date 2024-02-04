namespace SwiftLink.Application.UseCases.Links;
internal record LinkMessages
{
    public static readonly Error ExpirationDateMustBeMoreThanTomorrow = Error.Validation("ExpirationDate Invalid", "ExpirationDate Must Be More Than Tomorrow!");
    public static readonly Error UrlMustBeSent = Error.Validation("InValidFormat", "Url must be sent!");
    public static readonly Error InvalidUrlFormat = Error.Validation("InValidFormat", "Invalid Url Format!");
    public static readonly Error LinkIsBanned = Error.Validation("LinkValidation", "Your Link in banned! :(");
    public static readonly Error LinkIsExpired = Error.Validation("LinkValidation", "Your Link is Expired! :(");
    public static readonly Error PasswordIsNotSent = Error.Validation("LinkValidation", "This Link is protected, Please Send your password!");
    public static readonly Error InvalidPassword = Error.Validation("LinkValidation", "Invalid Password!");
    public static readonly Error LinkIsNotFound = Error.Validation("ShortCodeValidation", "Invalid ShortCode");
    public static readonly Error InvalidLinkId = Error.Validation("LinkValidation", "Invalid LinkId or Subscriber Token.");
}
