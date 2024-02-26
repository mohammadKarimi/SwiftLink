using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using StackExchange.Redis;

namespace SwiftLink.Infrastructure.CacheProvider;

public static class PolicyExtensions
{
    public static void AddPollyPipelines(this IServiceCollection services)//this IPolicyRegistry<string> policy)
    {
        services.AddResiliencePipeline<string, string>("my-key", builder =>
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

        //policy.Add("", circuitBreaker);

        //var setCacheCircuitBreaker =
        //    Policy<bool>.HandleResult(false).CircuitBreakerAsync(1, TimeSpan.FromSeconds(60));

        //var getCacheCircuitBreaker =
        //    Policy<string>.HandleResult((r) => { return r is null; }).CircuitBreakerAsync(1, TimeSpan.FromSeconds(60));

        //policy.Add(nameof(RedisCashServiceResiliencyKey.SetOrRemoveCircuitBreaker), setCacheCircuitBreaker);
        //policy.Add(nameof(RedisCashServiceResiliencyKey.GetCircuitBreaker), getCacheCircuitBreaker);
    }
}
