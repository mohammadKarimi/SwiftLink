namespace SwiftLink.Application.Common.Interfaces;

/// <summary>
/// This is a shared context between MediatR Behaviors and RequestHandlers.
/// </summary>
internal interface ISharedContext
{
    /// <summary>
    /// Set value in Shared Context.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void Set<TKey, TValue>(TKey key, TValue value);
}
