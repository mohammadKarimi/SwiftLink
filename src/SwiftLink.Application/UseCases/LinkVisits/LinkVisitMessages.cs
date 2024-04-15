namespace SwiftLink.Application.UseCases.LinkVisits;

internal record LinkVisitMessages
{
    public static readonly Error LinkIdMustBeSent = Error.Validation("LinkIdValidation", "LinkId Can Not Be Null!");
}
