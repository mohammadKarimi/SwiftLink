using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.Dtos;


namespace SwiftLink.Application.UseCases.Links.Queries;

public class GetLinkByGroupNameQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetLinkByGroupNameQuery, Result<IEnumerable<LinksDto>>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IEnumerable<LinksDto>>> Handle(GetLinkByGroupNameQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<LinksDto> result = await _dbContext.Set<Link>()
                               .Where(x => x.GroupName == request.GroupName)
                               .AsNoTracking()
                               .Select(x => new LinksDto
                               {
                                   Description = x.Description,
                                   ExpirationDate = x.ExpirationDate,

                                   IsBanned = x.IsBanned,
                                   OriginalUrl = x.OriginalUrl,
                                   ShortCode = x.ShortCode,
                                   IsDisabled = x.IsDisabled,
                                   LinkdId = x.Id
                               })
                               .ToListAsync(cancellationToken);
        return Result.Success(result);
    }
}
