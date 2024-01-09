namespace SwiftLink.Application.Common.Interfaces;

/// <summary>
/// This interface is designed to asynchronously generate a short code based on a provided original URL.
/// </summary>
public interface IShortCodeGenerator
{
    Task<string> GenerateAsync(string originalUrl, CancellationToken cancellationToken = default);
}
