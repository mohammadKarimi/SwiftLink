using SwiftLink.Application.Common.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SwiftLink.Application.Common;

public class TimeBasedShortCodeGenerator : IShortCodeGenerator
{
    private static readonly object _lockObject = new();
    private static readonly Random _random = new();
    private const byte COUNT_OF_RANDOM_NUMBER = 8;
    private const byte COUNT_OF_HASH_SPLITTER = 8;

    public string Generate(string originalUrl)
        => $"{GetRandomString(COUNT_OF_RANDOM_NUMBER)}{GetHash(originalUrl)}";

    private static string GetRandomString(int length)
    {
        lock (_lockObject)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }

    private static string GetHash(string input)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(hashBytes).Replace("-", string.Empty)[..COUNT_OF_HASH_SPLITTER];
    }
}
