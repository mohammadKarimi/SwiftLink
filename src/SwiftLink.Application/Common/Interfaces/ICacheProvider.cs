namespace SwiftLink.Application.Common.Interfaces;

/// <summary>
/// RedisCacheService implements the ICacheProvider interface and provides caching functionality using a distributed cache.
/// </summary>
/// <remarks>
/// Initializes a new instance of the RedisCacheService class.
/// </remarks>
public interface ICacheProvider
{
    /// <summary>
    /// Sets a string value in the cache with a specified expiration date.
    /// </summary>
    /// <param name="key">The key under which the value will be stored.</param>
    /// <param name="value">The string value to be stored in the cache.</param>
    /// <param name="expirationDate">The expiration date for the cached item.</param>
    /// <returns>A task representing the asynchronous operation, returning true if the operation was successful.</returns>
    public Task<bool> Set(string key, string value, DateTime expirationDate);

    /// <summary>
    /// Associate a value with a key in the Cache such as Redis.
    /// </summary>
    /// <param name="key">The key of the entry to add.</param>
    /// <param name="value">The value to associate with the key.</param>
    /// <returns>The value that was set or not.</returns>
    public Task<bool> Set(string key, string value);

    /// <summary>
    /// Removes the object associated with the given key.
    /// </summary>
    /// <param name="key">An object identifying the requested entry.</param>
    Task Remove(string key);

    /// <summary>
    /// Gets the item associated with this key if present.
    /// </summary>
    /// <param name="key">An object identifying the requested entry.</param>
    /// <returns>The located value or default url</returns>
    Task<string> Get(string key);
}