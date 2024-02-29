namespace SwiftLink.Application.UseCases.Links;
internal record LinkMessages
{
    public static readonly Error ExpirationDateMustBeMoreThanTomorrow = Error.Validation("ExpirationDate Validation", "ExpirationDate Must Be More Than Tomorrow!");
    public static readonly Error ExpirationDateIsRequiredToSetReminder = Error.Validation("ExpirationDate Validation", "Expiration date is required when setting a reminder.");
    public static readonly Error InvalidReminderDate = Error.Validation("ReminderDate Validation", "The Reminder time should be between now and expirationDate");
    public static readonly Error UrlMustBeSent = Error.Validation("InValidFormat", "Url must be sent!");
    public static readonly Error InvalidUrlFormat = Error.Validation("InValidFormat", "Invalid Url Format!");
    public static readonly Error LinkIsBanned = Error.Validation("LinkValidation", "Your Link in banned! :(");
    public static readonly Error LinkIsDisabledByUser = Error.Validation("LinkValidation", "Your Link in Disabled! :(");
    public static readonly Error LinkIsExpired = Error.Validation("LinkValidation", "Your Link is Expired! :(");
    public static readonly Error PasswordIsNotSent = Error.Validation("LinkValidation", "This Link is protected, Please Send your password!");
    public static readonly Error InvalidPassword = Error.Validation("LinkValidation", "Invalid Password!");
    public static readonly Error LinkIsNotFound = Error.Validation("ShortCodeValidation", "Invalid ShortCode");
    public static readonly Error InvalidLinkId = Error.Validation("LinkValidation", "Invalid LinkId or Subscriber Token.");
    public static readonly Error InvalidSubscriberId = Error.Validation("LinkValidation", "Invalid SubscriberId.");
    public static readonly Error BackHalfIsExist = Error.Failure("LinkValidation", "This back-half text is used before.");
    public static readonly Error BackHalfLength = Error.Validation("BackHalf Validation", "Back-Half must be less than 16 characters.");
    public static readonly Error BackHalfInvalidFormat = Error.Validation("BackHalf Validation", "Back-Half must be contain words and numbers.");
    public static readonly Error GroupNameMustBeSent = Error.Validation("GroupName Validation", "GroupName field can not be null.");
    public static readonly Error InvalidBackHalfLength = Error.Validation("LinkValidation", "maximum length for back-half text is 30.");
    public static readonly Error InvalidBackHalf = Error.Validation("LinkValidation", "maximum length for back-half text is 30.");
}
