using MediatR;
using Microsoft.Extensions.Options;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;
using System.Text.Json;

namespace SwiftLink.Application.UseCases.Links.Commands;

public class GenerateShortCodeCommandHandler(IApplicationDbContext dbContext,
                                             ICacheProvider cacheProvider,
                                             IShortCodeGenerator codeGenerator,
                                             IOptions<AppSettings> options,
                                             ISharedContext sharedContext)
    : IRequestHandler<GenerateShortCodeCommand, Result<object>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly ICacheProvider _cache = cacheProvider;
    private readonly IShortCodeGenerator _codeGenerator = codeGenerator;
    private readonly ISharedContext _sharedContext = sharedContext;
    private readonly AppSettings _options = options.Value;

    public async Task<Result<object>> Handle(GenerateShortCodeCommand request, CancellationToken cancellationToken = default)
    {
        var _linkTable = _dbContext.Set<Link>();

        var link = await _linkTable.FirstOrDefaultAsync(x => x.OriginalUrl == request.Url, cancellationToken);
        if (link is null)
        {
            link = new Link
            {
                OriginalUrl = request.Url,
                ShortCode = _codeGenerator.Generate(request.Url),
                Description = request.Description,
                SubscriberId = int.Parse(_sharedContext.Get(nameof(Subscriber.Id)).ToString()),
                ExpirationDate = request.ExpirationDate ?? DateTime.Now.AddDays(_options.DefaultExpirationTimeInDays),
                Password = request.Password is not null ? PasswordHasher.HashPassword(request.Password, request.Url) : null
            };

            _linkTable.Add(link);
            var dbResult = await _dbContext.SaveChangesAsync(cancellationToken);

            if (dbResult.IsFailure)
                return Result.Failure<object>(CommonMessages.Database.InsertFailed);
        }

        await _cache.Set(link.ShortCode, JsonSerializer.Serialize(link), link.ExpirationDate);

        return Result.Success<object>(new
        {
            link.ExpirationDate,
            link.IsBanned,
            link.ShortCode,
            link.OriginalUrl,
        });
    }
}
