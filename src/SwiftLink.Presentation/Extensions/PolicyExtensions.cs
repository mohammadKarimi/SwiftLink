using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;
using SwiftLink.Infrastructure.CacheProvider;

namespace SwiftLink.Presentation.Extensions;

public static class PolicyExtensions
{
    public static void AddPolicies(this IPolicyRegistry<string> policy)
    {
        AsyncCircuitBreakerPolicy<bool> setCacheCircuitBreaker =
            Policy<bool>.HandleResult(false).CircuitBreakerAsync(1, TimeSpan.FromSeconds(60));

        AsyncCircuitBreakerPolicy<string> getCacheCircuitBreaker =
            Policy<string>.HandleResult((r) => { return r is null; }).CircuitBreakerAsync(1, TimeSpan.FromSeconds(60));

        policy.Add(nameof(RedisCashServiceResiliencyKey.SetCircuitBreaker), setCacheCircuitBreaker);
        policy.Add(nameof(RedisCashServiceResiliencyKey.GetCircuitBreaker), getCacheCircuitBreaker);
    }
}
