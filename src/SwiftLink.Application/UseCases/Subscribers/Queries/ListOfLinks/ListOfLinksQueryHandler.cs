using System.Text.Json;
using MediatR;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.Notifications;
using SwiftLink.Application.UseCases.Subscribers.Queries.ListOfLinks;
using SwiftLink.Domain.Entities;

namespace SwiftLink.Application.UseCases.Links.Queries.VisitShortenLink;

public class ListOfLinksQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListOfLinksQuery, Result<IReadOnlyList<ListOfLinksDto>>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<IReadOnlyList<ListOfLinksDto>>> Handle(ListOfLinksQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<Link>()
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Take(request.Count)
            .Select(x => new ListOfLinksDto
            {
                LinkdId = x.Id,
                Description = x.Description,
                ExpirationDate = x.ExpirationDate,
                IsBanned = x.IsBanned,
                OriginalUrl = x.OriginalUrl,
                ShortCode = x.ShortCode
            })
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<ListOfLinksDto>>(result);
    }
}