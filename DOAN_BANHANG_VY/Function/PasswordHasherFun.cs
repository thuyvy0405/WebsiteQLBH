using System;
using System.Security.Cryptography;
using System.Text;

namespace QLBTBD.Function
{
    public class PasswordHasherFun
    {
        // Number of iterations for the hashing algorithm (adjust according to your security requirements)
        private const int Iterations = 10000;


        // Generate a random 6-digit password, hash it, and return the hashed value
        public static string GenerateAndHashRandomPassword()
        {
            string randomPassword = GenerateRandomPassword();
            return HashPassword(randomPassword);
        }

        // Generate a random 6-digit password
        public static string GenerateRandomPassword()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        // Generate a salt and hash the password
        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine the salt and hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Convert to Base64 and return as a string
            return Convert.ToBase64String(hashBytes);
        }

        // Verify a hashed password against a provided password
        public static bool VerifyPassword(string storedHash, string password)
        {
            // Convert the stored hash from Base64 to bytes
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Extract the salt from the stored hash
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Hash the provided password with the stored salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the computed hash with the stored hash
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false; // Passwords don't match
                }
            }

            return true; // Passwords match
        }
    }
}
