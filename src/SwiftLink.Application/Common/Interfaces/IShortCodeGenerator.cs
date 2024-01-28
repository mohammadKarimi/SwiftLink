namespace SwiftLink.Application.Common.Interfaces;

/// <summary>
/// This interface is designed to asynchronously generate a short code based on a provided original URL.
/// </summary>
public interface IShortCodeGenerator
{
    /// <summary>
    /// Generate 16 charachter for an originalUrl and return shortcode.
    /// </summary>
    /// <param name="originalUrl">Url which we want to map to a short code.</param>
    /// <returns>16 charachters.</returns>
    string Generate(string originalUrl);
}