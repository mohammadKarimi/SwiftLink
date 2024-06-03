using System.Net;
using Microsoft.Extensions.Options;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.Dtos;

namespace SwiftLink.Application.UseCases.Links.Commands;

public class GenerateShortCodeCommandHandler(IApplicationDbContext dbContext,
                                             IShortCodeGenerator codeGenerator,
                                             IOptions<AppSettings> options,
                                             ISharedContext sharedContext)
    : IRequestHandler<GenerateShortCodeCommand, Result<LinksDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IShortCodeGenerator _codeGenerator = codeGenerator;
    private readonly ISharedContext _sharedContext = sharedContext;
    private readonly IOptions<AppSettings> _options = options;

    public async Task<Result<LinksDto>> Handle(GenerateShortCodeCommand request,
        CancellationToken cancellationToken = default)
    {
        var linkTable = _dbContext.Set<Link>();
        Link link = new()
        {
            OriginalUrl = request.Url,
            ShortCode = request.BackHalf ?? _codeGenerator.Generate(request.Url),
            Description = request.Description,
            SubscriberId = int.Parse(_sharedContext.Get(nameof(Subscriber.Id)).ToString()),
            ExpirationDate = request.ExpirationDate ?? DateTime.Now.AddDays(_options.Value.DefaultExpirationTimeInDays),
            Password = request.Password?.Hash(request.Url),
            Title = request.Title,
            GroupName = request.GroupName
        };
        link.AddTags(request.Tags);
        if (request.RemindDate is not null)
            link.AddReminder(request.RemindDate.Value);

        linkTable.Add(link);
        var saveChangesResult = await _dbContext.SaveChangesAsync(cancellationToken);


        return saveChangesResult.IsFailure
            ? Result.Failure<LinksDto>(CommonMessages.Database.InsertFailed)
            : Result.Success(new LinksDto()
            {
                ExpirationDate = link.ExpirationDate,
                IsBanned = link.IsBanned,
                ShortCode = link.ShortCode,
                OriginalUrl = link.OriginalUrl,
                Description = link.Description,
                LinkdId = link.Id
            });
    }
}
