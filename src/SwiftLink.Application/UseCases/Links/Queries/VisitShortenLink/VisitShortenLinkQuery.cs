using MediatR;

namespace SwiftLink.Application.UseCases.Links.Queries.VisitShortCode;

public record VisitShortenLinkQuery(string Url, string Password) : IRequest<Result<string>>;
