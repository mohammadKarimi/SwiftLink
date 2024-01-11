using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text;

namespace SwiftLink.Infrastructure.CacheProvider;

public class RedisCacheService(IDistributedCache cache, IOptions<AppSettings> options)
    : ICacheProvider
{
    private readonly IDistributedCache _cache = cache;
    private readonly AppSettings _options = options.Value;

    public Task Remove(string key)
        => _cache.RemoveAsync(key);

    public async Task<bool> Set(string key, string value)
        => await Set(key, value, DateTime.Now.AddDays(_options.DefaultExpirationTimeInDays));

    public async Task<bool> Set(string key, string value, DateTime expirationDate)
    {
        DistributedCacheEntryOptions cacheEntryOptions = new()
        {
            SlidingExpiration = TimeSpan.FromHours(_options.Redis.SlidingExpirationHour),
            AbsoluteExpiration = expirationDate,
        };

        var dataToCache = Encoding.UTF8.GetBytes(value);
        await _cache.SetAsync(key, dataToCache, cacheEntryOptions);
        return true;
    }

    public async Task<string> Get(string key)
        => await _cache.GetStringAsync(key);
}
