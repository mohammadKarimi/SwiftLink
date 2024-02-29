using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Domain.Entities.Link;

namespace SwiftLink.Application.UseCases.Links.Queries;
public class InquiryBackHalfHanlder(IApplicationDbContext dbContext) : IRequestHandler<InquiryBackHalfQuery, Result<bool>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<bool>> Handle(InquiryBackHalfQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<Link>()
                               .Where(x => x.ShortCode == request.BackHalfText)
                               .AsNoTracking()
                               .AnyAsync(cancellationToken);

        return result ? Result.Failure<bool>(LinkMessages.BackHalfIsExist) : Result.Success(true);
    }
}
