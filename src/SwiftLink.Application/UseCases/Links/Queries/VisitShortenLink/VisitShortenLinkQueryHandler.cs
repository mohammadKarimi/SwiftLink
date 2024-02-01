using System.Text.Json;
using MediatR;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.Notifications;

namespace SwiftLink.Application.UseCases.Links.Queries;

public class CountVisitShortenLinkQueryHandler(IApplicationDbContext dbContext,
                                          ICacheProvider cacheProvider,
                                          IMediator mediator)
    : IRequestHandler<VisitShortenLinkQuery, Result<string>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly ICacheProvider _cache = cacheProvider;
    private readonly IMediator _mediator = mediator;

    public async Task<Result<string>> Handle(VisitShortenLinkQuery request, CancellationToken cancellationToken)
    {
        Link link;
        var cacheResult = await _cache.Get(request.ShortCode);
        if (!string.IsNullOrEmpty(cacheResult))
            link = JsonSerializer.Deserialize<Link>(cacheResult);
        else
        {
            link = await _dbContext.Set<Link>().FirstOrDefaultAsync(x => x.ShortCode == request.ShortCode, cancellationToken);
            if (link is null)
                return Result.Failure<string>(LinkMessages.LinkIsNotFound);
            await _cache.Set(request.ShortCode, JsonSerializer.Serialize(link));
        }

        if (link.IsBanned)
            return Result.Failure<string>(LinkMessages.LinkIsBanned);

        if (link.ExpirationDate <= DateTime.Now)
            return Result.Failure<string>(LinkMessages.LinkIsExpired);

        if (link.Password is not null)
        {
            if (request.Password is null)
            {
                return Result.Failure<string>(LinkMessages.PasswordIsNotSent);
            }
            else if (request.Password.Hash(link.OriginalUrl) != link.Password)
                return Result.Failure<string>(LinkMessages.InvalidPassword);
        }

        await _mediator.Publish(new VisitLinkNotification
        {
            LinkId = link.Id,
            ClientMetaData = request.ClientMetaData
        }, cancellationToken);

        return Result.Success(link.OriginalUrl);
    }
}