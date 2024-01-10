using System.Security.Cryptography;
using System.Text;

namespace SwiftLink.Application.Common;

public class PasswordHasher
{
    private const int SaltSize = 32;

    public static string HashPassword(string password)
    {
        byte[] salt = GenerateSalt();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
        Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

        using SHA256 sha256 = SHA256.Create();
        byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

        byte[] hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
        Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, 0, hashedBytes.Length);
        Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, hashedBytes.Length, salt.Length);

        return Convert.ToBase64String(hashedPasswordWithSalt);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        byte[] hashedPasswordWithSalt = Convert.FromBase64String(hashedPassword);
        byte[] salt = new byte[SaltSize];

        Buffer.BlockCopy(hashedPasswordWithSalt, hashedPasswordWithSalt.Length - SaltSize, salt, 0, SaltSize);
        string hashedEnteredPassword = HashPasswordWithGivenSalt(password, salt);
        
        return hashedEnteredPassword.Equals(hashedPassword);
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[SaltSize];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

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
