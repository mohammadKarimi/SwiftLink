using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace SwiftLink.Infrastructure.CacheProvider;

public class RedisCacheService(IDistributedCache cache) : ICacheProvider
{
    private readonly IDistributedCache _cache = cache;
    
    public Task Remove(string key)
         => _cache.RemoveAsync(key);

    public async Task<bool> Set(string key, string value)
    {
        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(3));
        var dataToCache = Encoding.UTF8.GetBytes(value);
        await _cache.SetAsync(key, dataToCache, options);
        return true;
    }

    public async Task<string> Get(string key)
        =>await _cache.GetStringAsync(key);
}
