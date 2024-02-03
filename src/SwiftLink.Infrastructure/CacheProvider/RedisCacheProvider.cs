using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using StackExchange.Redis;
using System.Text;

namespace SwiftLink.Infrastructure.CacheProvider;

public class RedisCacheService(IDistributedCache cache, IOptions<AppSettings> options)
    : ICacheProvider
{

    private readonly AsyncCircuitBreakerPolicy<bool> circuitBreakerPolicy = Policy<bool>.HandleResult(false)
                                                                                        .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

    private readonly IDistributedCache _cache = cache;
    private readonly AppSettings _options = options.Value;

    public Task Remove(string key)
        => _cache.RemoveAsync(key);

    public async Task<bool> Set(string key, string value)
        => await Set(key, value, DateTime.Now.AddDays(_options.DefaultExpirationTimeInDays));

    public async Task<bool> Set(string key, string value, DateTime expirationDate)
    {
        if (circuitBreakerPolicy.CircuitState is CircuitState.Open)
            return false;
        return await circuitBreakerPolicy.ExecuteAsync(async () =>
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
        try
        {
            return await _cache.GetStringAsync(key);
        }
        catch
        {
            return null;
        }
    }
}
