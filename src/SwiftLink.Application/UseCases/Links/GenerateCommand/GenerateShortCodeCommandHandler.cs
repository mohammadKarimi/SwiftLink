using MediatR;
using Microsoft.Extensions.Options;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;
using System.Text.Json;

namespace SwiftLink.Application.UseCases.Links.GenerateCommand;

public class GenerateShortCodeCommandHandler(IApplicationDbContext dbContext,
                                             ICacheProvider cacheProvider,
                                             IShortCodeGenerator codeGenerator,
                                             IOptions<AppSettings> options)
    : IRequestHandler<GenerateShortCodeCommand, Result<object>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly ICacheProvider _cache = cacheProvider;
    private readonly IShortCodeGenerator _codeGenerator = codeGenerator;
    private readonly AppSettings _options = options.Value;

    public async Task<Result<object>> Handle(GenerateShortCodeCommand request, CancellationToken cancellationToken)
    {
        var link = new Link
        {
            OriginalUrl = request.Url,
            ShortCode = _codeGenerator.Generate(request.Url),
            Description = request.Description,
            SubscriberId = 1,
            ExpirationDate = request.ExpirationDate ?? DateTime.Now.AddDays(_options.DefaultExpirationTimeInDays),
            Password = request.Password is not null ? PasswordHasher.HashPassword(request.Password) : null
        };

        _dbContext.Set<Link>().Add(link);

        var dbResult = await _dbContext.SaveChangesAsync(cancellationToken);
        if (dbResult.IsFailure)
            return Result<object>.Failure();

        await _cache.Set(request.Url, JsonSerializer.Serialize(link), link.ExpirationDate);
        return Result<object>.Success(link);
    }
}
