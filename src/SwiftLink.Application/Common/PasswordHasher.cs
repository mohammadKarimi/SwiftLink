using System.Security.Cryptography;
using System.Text;

namespace SwiftLink.Application.Common;

public class PasswordHasher
{
    public static string HashPassword(string password, string salt)
     => HashPasswordWithGivenSalt(password, Encoding.UTF8.GetBytes(salt));

    private static string HashPasswordWithGivenSalt(string password, byte[] salt)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
        Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);
        byte[] hashedBytes = SHA256.HashData(saltedPassword);
        return Convert.ToBase64String(hashedBytes);
    }
}
