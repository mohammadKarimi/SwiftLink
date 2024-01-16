using MediatR;
using Microsoft.Extensions.Options;
using SwiftLink.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftLink.Application.UseCases.Links.Queries.VisitShortCode;
public class VisitShortenLinkQueryHandler(IApplicationDbContext dbContext,
                                             ICacheProvider cacheProvider,
                                             IShortCodeGenerator codeGenerator,
                                             IOptions<AppSettings> options,
                                             ISharedContext sharedContext)
    : IRequestHandler<VisitShortenLinkQuery, Result<string>>
{

    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly ICacheProvider _cache = cacheProvider;
    private readonly IShortCodeGenerator _codeGenerator = codeGenerator;
    private readonly ISharedContext _sharedContext = sharedContext;
    private readonly AppSettings _options = options.Value;

    public Task<Result<string>> Handle(VisitShortenLinkQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
