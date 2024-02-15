using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;
using Polly.Retry;
using StackExchange.Redis;
using System.Security.Cryptography;

namespace SwiftLink.Infrastructure.CacheProvider;

public class RedisCacheService(IDistributedCache cache, IOptions<AppSettings> options, IReadOnlyPolicyRegistry<string> policyRegistry)
    : ICacheProvider
{
    private readonly IDistributedCache _cache = cache;
    private readonly IReadOnlyPolicyRegistry<string> _policyRegistry = policyRegistry;

    private readonly AppSettings _options = options.Value;

    public async Task<bool> Remove(string key)
    {

        var removeCacheCircuitBreaker = _policyRegistry.Get<AsyncCircuitBreakerPolicy<bool>>(nameof(RedisCashServiceResiliencyKey.SetOrRemoveCircuitBreaker));
        return removeCacheCircuitBreaker.CircuitState is not CircuitState.Open &&
            await removeCacheCircuitBreaker.ExecuteAsync(async () =>
            {
                try
                {
                    await _cache.RemoveAsync(key);
                }
                catch (RedisConnectionException)
                {
                    return false;
                }
                return true;
            });
    }

    public async Task<bool> Set(string key, string value)
        => await Set(key, value, DateTime.Now.AddDays(_options.DefaultExpirationTimeInDays));

    public async Task<bool> Set(string key, string value, DateTime expirationDate)
    {
        var setCacheCircuitBreaker = _policyRegistry.Get<AsyncCircuitBreakerPolicy<bool>>(nameof(RedisCashServiceResiliencyKey.SetOrRemoveCircuitBreaker));
        return setCacheCircuitBreaker.CircuitState is not CircuitState.Open &&
            await setCacheCircuitBreaker.ExecuteAsync(async () =>
            {
                try
                {
                    DistributedCacheEntryOptions cacheEntryOptions = new()
                    {
                        SlidingExpiration = TimeSpan.FromHours(_options.Redis.SlidingExpirationHour),
                        AbsoluteExpiration = expirationDate,
                    };
                    var dataToCache = Encoding.UTF8.GetBytes(value);
                    await _cache.SetAsync(key, dataToCache, cacheEntryOptions);
                }
                catch (RedisConnectionException)
                {
                    return false;
                }
                return true;
            });
    }

    public async Task<string> Get(string key)
    {
        var getCacheCircuitBreaker = _policyRegistry.Get<AsyncCircuitBreakerPolicy<string>>(nameof(RedisCashServiceResiliencyKey.GetCircuitBreaker));
        return getCacheCircuitBreaker.CircuitState is CircuitState.Open
            ? null
            : await getCacheCircuitBreaker.ExecuteAsync(async () =>
                {
                    try
                    {
                        return await _cache.GetStringAsync(key);
                    }
                    catch (RedisConnectionException)
                    {
                        return null;
                    }
                });
    }
}

public enum RedisCashServiceResiliencyKey
{
    SetOrRemoveCircuitBreaker,
    GetCircuitBreaker
}
