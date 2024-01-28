using MediatR;
using SwiftLink.Application.Common.Exceptions;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.Common.Security;
using System.Reflection;

namespace SwiftLink.Application.Behaviors;
internal class SubscriberAuthorizationBehavior<TRequest, TResponse>(IApplicationDbContext dbContext,
                                                            ISharedContext sharedContext)
    : IPipelineBehavior<TRequest, TResponse>
{

    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly ISharedContext _sharedContext = sharedContext;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();
        var result = await _dbContext.Set<Subscriber>()
            .FirstOrDefaultAsync(x => x.Token == authorizationAttributes.First().Token && x.IsActive, cancellationToken) ?? throw new SubscriberUnAuthorizedException();

        _sharedContext.Set(nameof(result.Id), result.Id);
        return await next();
    }
}
