namespace SwiftLink.Application.UseCases.Links;
internal record LinkMessages
{
    public static Error UrlMustBeSent = Error.Validation("InValidFormat", "Url must be sent!");

    public static Error InvalidUrlFormat = Error.Validation("InValidFormat", "Invalid Url Format!");
}
