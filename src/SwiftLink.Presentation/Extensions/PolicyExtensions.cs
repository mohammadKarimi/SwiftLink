using Polly;
using Polly.Registry;
using SwiftLink.Infrastructure.CacheProvider;

namespace SwiftLink.Presentation.Extensions;

public static class PolicyExtensions
{
    public static void AddPolicies(this IPolicyRegistry<string> policy)
    {
        var setCacheCircuitBreaker =
            Policy<bool>.HandleResult(false).CircuitBreakerAsync(1, TimeSpan.FromSeconds(60));

        var getCacheCircuitBreaker =
            Policy<string>.HandleResult((r) => { return r is null; }).CircuitBreakerAsync(1, TimeSpan.FromSeconds(60));

        policy.Add(nameof(RedisCashServiceResiliencyKey.SetOrRemoveCircuitBreaker), setCacheCircuitBreaker);
        policy.Add(nameof(RedisCashServiceResiliencyKey.GetCircuitBreaker), getCacheCircuitBreaker);
    }
}
