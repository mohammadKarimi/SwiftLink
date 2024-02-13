using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.Dtos;
using SwiftLink.Application.UseCases.Subscribers.Queries;

namespace SwiftLink.Application.UseCases.Links.Queries;

public class ListOfLinksQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListOfLinksQuery, Result<IReadOnlyList<LinksDto>>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IReadOnlyList<LinksDto>>> Handle(ListOfLinksQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<Link>()
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Take(request.Count)
            .Select(x => new LinksDto
            {
                LinkdId = x.Id,
                Description = x.Description,
                ExpirationDate = x.ExpirationDate,
                IsBanned = x.IsBanned,
                OriginalUrl = x.OriginalUrl,
                ShortCode = x.ShortCode,
                IsDisabled = x.IsDisabled
            })
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<LinksDto>>(result);
    }
}
