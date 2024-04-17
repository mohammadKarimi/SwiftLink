using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.UseCases.LinkVisits.Queries;

public class GetClientMetaDataByLinkIdQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetClientMetaDataByLinkIdQuery, Result<IEnumerable<LinkVisitDto>>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IEnumerable<LinkVisitDto>>> Handle(GetClientMetaDataByLinkIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<LinkVisitDto> result = await _dbContext.Set<LinkVisit>()
                                     .Where(x => x.LinkId == request.LinkId)
                                     .Skip(100)
                                     .OrderByDescending(x => x.Id)
                                     .AsNoTracking()
                                     .Select(x => new LinkVisitDto
                                     {
                                         ClientMetaDate = x.ClientMetaData,
                                         Date = x.Date
                                     })
                                     .ToListAsync(cancellationToken);
        return Result.Success(result);
    }
}
