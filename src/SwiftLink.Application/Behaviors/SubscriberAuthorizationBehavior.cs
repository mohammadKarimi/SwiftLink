using MediatR;
using SwiftLink.Application.Common.Exceptions;
using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.Behaviors;
internal class SubscriberAuthorizationBehavior<TRequest, TResponse>(IApplicationDbContext dbContext,
                                                                    ISharedContext sharedContext,
                                                                    IUser user)
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly ISharedContext _sharedContext = sharedContext;
    private readonly IUser _user = user;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_user.Token is null)
            throw new SubscriberUnAuthorizedException();

        var result = await _dbContext.Set<Subscriber>().FirstOrDefaultAsync(x => x.Token == _user.Token
                                                                                 && x.IsActive, cancellationToken) ?? throw new SubscriberUnAuthorizedException();

        _sharedContext.Set(nameof(result.Id), result.Id);
        return await next();
    }
}
