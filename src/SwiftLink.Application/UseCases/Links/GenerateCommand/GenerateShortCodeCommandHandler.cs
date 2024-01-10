using MediatR;
using Microsoft.Extensions.Options;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;

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
        var link = new Link()
        {
            OriginalUrl = request.Url,
            Password = PasswordHasher.HashPassword(request.Password),
            ShortCode = _codeGenerator.Generate(request.Url),
            ExpirationDate = request.ExpirationDate is null ? DateTime.Now.AddDays(_options.DefaultExpirationTimeInDays) : request.ExpirationDate.Value,
            Description = request.Description,
            SubscriberId = 1
        };

        _dbContext.Set<Link>().Add(link);
        var dbResult = await _dbContext.SaveChangesAsync(cancellationToken);
        if (dbResult.IsFailure)
            return Result<object>.Failure();

        await _cache.Set(request.Url, link.ShortCode);
        return Result<object>.Success(link);
    }
}