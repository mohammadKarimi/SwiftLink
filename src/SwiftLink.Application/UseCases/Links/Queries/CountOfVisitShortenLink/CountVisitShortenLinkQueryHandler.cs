using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.Dtos;

namespace SwiftLink.Application.UseCases.Links.Queries;

public class CountVisitShortenLinkQueryHandler(IApplicationDbContext dbContext,
                                               ISharedContext sharedContext)
    : IRequestHandler<CountVisitShortenLinkQuery, Result<CountOfVisitLinkDto>>
{
    private readonly ISharedContext _sharedContext = sharedContext;
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<CountOfVisitLinkDto>> Handle(CountVisitShortenLinkQuery request, CancellationToken cancellationToken)
    {
        var subscriberId = int.Parse(_sharedContext.Get(nameof(Subscriber.Id)).ToString());
        var link = await _dbContext.Set<Link>()
               .Where(x => x.Id == request.LinkId && x.SubscriberId == subscriberId)
               .Select(x => new CountOfVisitLinkDto
               {
                   ShortCode = x.ShortCode,
                   OriginalUrl = x.OriginalUrl
               })
               .FirstOrDefaultAsync(cancellationToken);

        if (link is null)
            return Result.Failure<CountOfVisitLinkDto>(LinkMessages.InvalidLinkId);

        var countOfVisit = await _dbContext.Set<LinkVisit>()
                                           .Where(x => x.LinkId == request.LinkId)
                                           .CountAsync(cancellationToken);
        link.Count = countOfVisit;
        return Result.Success(link);
    }
}
