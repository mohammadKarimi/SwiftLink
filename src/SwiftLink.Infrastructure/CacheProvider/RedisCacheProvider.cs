using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text;

namespace SwiftLink.Infrastructure.CacheProvider;

/// <summary>
/// RedisCacheService implements the ICacheProvider interface and provides caching functionality using a distributed cache.
/// </summary>
/// <remarks>
/// Initializes a new instance of the RedisCacheService class.
/// </remarks>
/// <param name="cache">The distributed cache implementation.</param>
/// <param name="options">Application settings injected as options.</param>
public class RedisCacheService(IDistributedCache cache, IOptions<AppSettings> options) 
    : ICacheProvider
{
    private readonly IDistributedCache _cache = cache;
    private readonly AppSettings _options = options.Value;

    /// <summary>
    /// Removes an item from the cache by its key.
    /// </summary>
    /// <param name="key">The key of the item to be removed from the cache.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Remove(string key)
        => _cache.RemoveAsync(key);

    /// <summary>
    /// Sets a string value in the cache with a default expiration time.
    /// </summary>
    /// <param name="key">The key under which the value will be stored.</param>
    /// <param name="value">The string value to be stored in the cache.</param>
    /// <returns>A task representing the asynchronous operation, returning true if the operation was successful.</returns>
    public async Task<bool> Set(string key, string value)
        => await Set(key, value, DateTime.Now.AddDays(_options.DefaultExpirationTimeInDays));

    /// <summary>
    /// Sets a string value in the cache with a specified expiration date.
    /// </summary>
    /// <param name="key">The key under which the value will be stored.</param>
    /// <param name="value">The string value to be stored in the cache.</param>
    /// <param name="expirationDate">The expiration date for the cached item.</param>
    /// <returns>A task representing the asynchronous operation, returning true if the operation was successful.</returns>
    public async Task<bool> Set(string key, string value, DateTime expirationDate)
    {
        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(expirationDate)
            .SetSlidingExpiration(TimeSpan.FromHours(_options.Redis.SlidingExpirationHour));

        var dataToCache = Encoding.UTF8.GetBytes(value);
        await _cache.SetAsync(key, dataToCache, options);
        return true;
    }

    /// <summary>
    /// Gets a string value from the cache based on its key.
    /// </summary>
    /// <param name="key">The key of the item to retrieve from the cache.</param>
    /// <returns>A task representing the asynchronous operation, returning the string value from the cache.</returns>
    public async Task<string> Get(string key)
        => await _cache.GetStringAsync(key);
}
