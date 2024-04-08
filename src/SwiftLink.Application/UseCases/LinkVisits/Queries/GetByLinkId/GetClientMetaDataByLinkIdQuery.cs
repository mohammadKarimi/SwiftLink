namespace SwiftLink.Application.UseCases.LinkVisits.Queries;


public record GetClientMetaDataByLinkIdQuery(long LinkId) : IRequest<Result<IEnumerable<LinkVisitDto>>>;
