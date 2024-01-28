using MediatR;
using SwiftLink.Application.Common.Security;

namespace SwiftLink.Application.UseCases.Links.Queries.VisitShortenLink;

public record VisitShortenLinkQuery : IAnonymousRequest, IRequest<Result<string>>
{
    public string ShortCode { get; set; }
    public string Password { get; set; }
    public string ClientMetaData { get; set; }
}