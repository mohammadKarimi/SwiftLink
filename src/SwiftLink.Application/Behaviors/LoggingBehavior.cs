using MediatR;
using Microsoft.Extensions.Logging;
using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger, IUser user, IApplicationDbContext dbContext)
: IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger _logger = logger;
    private readonly IUser _user = user;
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var subscriberToken = _user.Token.ToString() ?? string.Empty;
        string subscriberName = string.Empty;

        if (!string.IsNullOrEmpty(subscriberToken))
        {
            var result = await _dbContext.Set<Subscriber>()
                .FirstOrDefaultAsync(x => x.Token == _user.Token && x.IsActive, cancellationToken);

            subscriberName = result.Name;
        }

        _logger.LogInformation("SwiftLink Request: {requestName} {@subscriberToken} {@subscriberName} {@Request}",
            requestName, subscriberToken, subscriberName, request);

        var response = await next();

        _logger.LogInformation("SwiftLink Response: {requestName} {@Response}",
        requestName, response);

        return response;
    }
}
