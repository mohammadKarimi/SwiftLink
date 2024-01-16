using MediatR;

namespace SwiftLink.Application.UseCases.Links.Queries.VisitShortCode;

public record VisitShortenLinkQuery(string ShortCode, string Password) : IRequest<Result<string>>;
