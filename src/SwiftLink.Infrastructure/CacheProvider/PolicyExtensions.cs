using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using StackExchange.Redis;

namespace SwiftLink.Infrastructure.CacheProvider;

public static class PolicyExtensions
{
    public static void AddPollyPipelines(this IServiceCollection services)
    {
        services.AddResiliencePipeline<string, string>(nameof(RedisCashServiceResiliencyKey.GetCircuitBreaker), builder =>
         {
             builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<string>()
             {
                 MinimumThroughput = 2,
                 FailureRatio = 0.1,
                 SamplingDuration = TimeSpan.FromSeconds(10),
                 BreakDuration = TimeSpan.FromSeconds(60),
                 ShouldHandle = new PredicateBuilder<string>().Handle<RedisConnectionException>()
             });
         });

        services.AddResiliencePipeline<string, bool>(nameof(RedisCashServiceResiliencyKey.SetOrRemoveCircuitBreaker), builder =>
        {
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<bool>()
            {
                MinimumThroughput = 2,
                FailureRatio = 0.1,
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromSeconds(60),
                ShouldHandle = new PredicateBuilder<bool>().Handle<RedisConnectionException>()
            });
        });
    }
}
