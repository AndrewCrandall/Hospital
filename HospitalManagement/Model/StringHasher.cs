// Author : Andrew Crandall
// Date Modified : 11/3/2024
// Title : StringHasher
// Purpose : Provide the logic for backend password hashing and validation

using System;
using System.Security.Cryptography;
using System.Text;

public static class StringHasher
{
    public static string HashString(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
    public static bool VerifyString(string input, string storedHashedValue)
    {
        string hashedInput = HashString(input);
        return storedHashedValue.Equals(hashedInput, StringComparison.OrdinalIgnoreCase);
    }
}
