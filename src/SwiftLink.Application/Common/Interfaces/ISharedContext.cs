namespace SwiftLink.Application.Common.Interfaces;

/// <summary>
/// This is a shared context between MediatR Behaviors and RequestHandlers.
/// </summary>
internal interface ISharedContext
{
    /// <summary>
    /// Set value in Shared Context.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    void Set(string key, string value);
}
