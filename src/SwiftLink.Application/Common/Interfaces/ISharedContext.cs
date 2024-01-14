namespace SwiftLink.Application.Common.Interfaces;

/// <summary>
/// This is a shared context between MediatR Behaviors and RequestHandlers.
/// </summary>
public interface ISharedContext
{
    /// <summary>
    /// Get Value From Shared Context.
    /// </summary>
    /// <param name="key">key</param>
    /// <returns></returns>
    string Get(string key);

    /// <summary>
    /// Set value in Shared Context.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    void Set(string key, string value);
}
