using System.Security.Cryptography;
using System.Text;

namespace SwiftLink.Application.Common;

public static class StringHasherExtension
{
    public static string Hash(this string value, string salt)
        => HashWithGivenSalt(value, Encoding.UTF8.GetBytes(salt));

    private static string HashWithGivenSalt(string value, byte[] salt)
    {
        byte[] valueBytes = Encoding.UTF8.GetBytes(value);
        byte[] saltedValue = new byte[valueBytes.Length + salt.Length];

        Buffer.BlockCopy(valueBytes, 0, saltedValue, 0, valueBytes.Length);
        Buffer.BlockCopy(salt, 0, saltedValue, valueBytes.Length, salt.Length);
        byte[] hashedBytes = SHA256.HashData(saltedValue);
        return Convert.ToBase64String(hashedBytes);
    }
}
