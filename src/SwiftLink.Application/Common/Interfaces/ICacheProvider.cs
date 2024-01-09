namespace SwiftLink.Application.Common.Interfaces;

public interface ICacheProvider
{
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
