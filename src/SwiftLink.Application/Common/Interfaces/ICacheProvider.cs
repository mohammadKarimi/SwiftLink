namespace SwiftLink.Application.Common.Interfaces;

internal interface ICacheProvider
{
    /// <summary>
    /// Associate a value with a key in the Cache such as Redis.
    /// </summary>
    /// <param name="key">The key of the entry to add.</param>
    /// <param name="value">The value to associate with the key.</param>
    /// <returns>The value that was set.</returns>
    public string Set(string key, string value);

    /// <summary>
    /// Removes the object associated with the given key.
    /// </summary>
    /// <param name="key">An object identifying the requested entry.</param>
    void Remove(string key);

    /// <summary>
    /// Gets the item associated with this key if present.
    /// </summary>
    /// <param name="key">An object identifying the requested entry.</param>
    /// <param name="value">The located value or default url.</param>
    /// <returns></returns>
    bool TryGetValue(string key, out string value);
}
