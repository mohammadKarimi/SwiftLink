using SwiftLink.Application.Dtos;

namespace SwiftLink.Application.UseCases.Links.Queries;
public record GetLinkByGroupNameQuery(string GroupName) : IRequest<Result<IEnumerable<LinksDto>>>;
