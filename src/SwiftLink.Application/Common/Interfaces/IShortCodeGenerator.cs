namespace SwiftLink.Application.Common.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IShortCodeGenerator
{
    Task<string> GenerateAsync(string originalUrl, CancellationToken cancellationToken = default);
}
