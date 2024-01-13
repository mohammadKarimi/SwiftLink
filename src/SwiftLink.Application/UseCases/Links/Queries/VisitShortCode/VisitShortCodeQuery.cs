using MediatR;

namespace SwiftLink.Application.UseCases.Links.Queries.VisitShortCode;

public record VisitShortCodeQuery(string Url) : IRequest<Link>;
