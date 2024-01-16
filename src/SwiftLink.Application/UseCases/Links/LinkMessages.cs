namespace SwiftLink.Application.UseCases.Links;
internal record LinkMessages
{
    public static readonly Error UrlMustBeSent = Error.Validation("InValidFormat", "Url must be sent!");

    public static readonly Error InvalidUrlFormat = Error.Validation("InValidFormat", "Invalid Url Format!");
}
