namespace SwiftLink.Application.UseCases.Links.Commands;

public class UpdateLinkCommandHandler(IApplicationDbContext dbContext,
                                      ISharedContext sharedContext,
                                      ICacheProvider cacheProvider)
    : IRequestHandler<UpdateLinkCommand, Result<bool>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly ISharedContext _sharedContext = sharedContext;
    private readonly ICacheProvider _cache = cacheProvider;

    public async Task<Result<bool>> Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        var link = await _dbContext.Set<Link>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (link is null)
            return Result.Failure<bool>(LinkMessages.LinkIsNotFound);

        if (link.SubscriberId != int.Parse(_sharedContext.Get(nameof(Subscriber.Id)).ToString()))
            return Result.Failure<bool>(LinkMessages.InvalidSubscriberId);

        link.Title = request.Title;

        var dbResult = await _dbContext.SaveChangesAsync(cancellationToken);

        if (dbResult.IsFailure)
            return Result.Failure<bool>(CommonMessages.Database.InsertFailed);

        await _cache.Remove(link.ShortCode);
        await _cache.Set(link.ShortCode, JsonSerializer.Serialize(link), link.ExpirationDate);

        return Result.Success(true);
    }
}
