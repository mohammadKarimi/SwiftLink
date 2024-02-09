using MediatR;
using Microsoft.Extensions.Logging;
using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger, ISharedContext sharedContext)
: IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger _logger = logger;
    private readonly ISharedContext _sharedContext = sharedContext;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation($"SwiftLink Request: {requestName} {_sharedContext.Get(nameof(Subscriber.Id))} {_sharedContext.Get(nameof(Subscriber.Name))} {request}");
        var response = await next();
        _logger.LogInformation($"SwiftLink Response: {requestName} {response}");

        return response;
    }
}
