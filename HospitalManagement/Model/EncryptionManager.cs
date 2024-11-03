// Author : Andrew Crandall
// Date Modified : 11/3/2024
// Title : encryptionManager
// Purpose : Provide the logic data encryption, generating new keys and IV's.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;

namespace HospitalManagement.Model
{
    public class EncryptionManager : SqlConnectionManager
    {
        public EncryptionManager() : base()
        {
            InitializeKeys();
        }

        private void InitializeKeys()
        {
            var (key, iv) = RetrieveLatestKeyData();
            if (key == null || iv == null)
            {
                GenerateAndStoreNewKey();
            }
        }

        private void GenerateAndStoreNewKey()
        {
            byte[] newKey = GenerateKey();
            byte[] iv = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            }
            StoreKey(newKey, iv);
        }

        public byte[] GenerateKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32]; // 256 bits
                rng.GetBytes(key);
                return key;
            }
        }

        private void StoreKey(byte[] key, byte[] iv)
        {
            if (!OpenConnection())
                throw new Exception("Could not open database connection.");

            using (var command = new SqlCommand("INSERT INTO EncryptionKeys (EncryptionKey, IV, CreatedAt) VALUES (@key, @iv, @createdAt)", GetConnection()))
            {
                command.Parameters.AddWithValue("@key", key);
                command.Parameters.AddWithValue("@iv", iv);
                command.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        public string Encrypt(string plainText, byte[] key, byte[] iv)
        {
            if (key == null || key.Length != 32)
                throw new ArgumentException("Invalid key size.");
            if (iv == null || iv.Length != 16)
                throw new ArgumentException("Invalid IV size.");

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var msEncrypt = new MemoryStream())
                {
                    // Write the IV to the beginning of the MemoryStream
                    msEncrypt.Write(iv, 0, iv.Length);

                    using (var csEncrypt = new CryptoStream(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                        swEncrypt.Flush(); // Flush the StreamWriter
                        csEncrypt.FlushFinalBlock(); // Finalize the encryption
                    }

                    // Return the full cipher text as a Base64 string
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            // Log the incoming cipher text
            Console.WriteLine($"Decrypting cipher text: {cipherText}");

            var fullCipher = Convert.FromBase64String(cipherText);

            // Check for minimum length
            if (fullCipher.Length < 16) // 16 bytes for IV
                throw new Exception("Cipher text is too short.");

            var iv = new byte[16];
            Array.Copy(fullCipher, iv, iv.Length);

            var cipher = new byte[fullCipher.Length - iv.Length];
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            var (key, retrievedIv) = RetrieveLatestKey();
            if (key == null || retrievedIv == null)
            {
                throw new Exception("No encryption key or IV found for decryption.");
            }

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var msDecrypt = new MemoryStream(cipher))
                using (var csDecrypt = new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }


        public (byte[] Key, byte[] IV) RetrieveLatestKey()
        {
            if (!OpenConnection())
                throw new Exception("Could not open database connection.");

            using (var command = new SqlCommand("SELECT TOP 1 EncryptionKey, IV FROM EncryptionKeys ORDER BY CreatedAt DESC", GetConnection()))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var key = reader["EncryptionKey"] as byte[];
                        var iv = reader["IV"] as byte[];
                        CloseConnection();
                        return (key, iv);
                    }
                }
            }
            CloseConnection();
            return (null, null);
        }

        private (byte[] Key, byte[] IV) RetrieveLatestKeyData()
        {
            if (!OpenConnection())
                throw new Exception("Could not open database connection.");

            using (var command = new SqlCommand("SELECT TOP 1 EncryptionKey, IV FROM EncryptionKeys ORDER BY CreatedAt DESC", GetConnection()))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var key = reader["EncryptionKey"] as byte[];
                        var iv = reader["IV"] as byte[];
                        CloseConnection();
                        return (key, iv);
                    }
                }
            }
            CloseConnection();
            return default;
        }

        public void RotateEncryptionKey()
        {
            GenerateAndStoreNewKey();
        }
    }
}
