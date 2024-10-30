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
            InitializeKeys(); // Initialize keys on instantiation
        }

        private void InitializeKeys()
        {
            var (key, iv) = RetrieveLatestKeyData(); // Unpack the tuple
                                                     // Only generate new keys if both key and IV are null
            if (key == null || iv == null)
            {
                GenerateAndStoreNewKey();
            }
        }


        private void GenerateAndStoreNewKey()
        {
            byte[] newKey = GenerateKey(); // Generate a new encryption key
            byte[] iv = new byte[16]; // 128 bits for AES
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv); // Generate a new IV
            }
            StoreKey(newKey, iv); // Store the new key and IV
        }

        public byte[] GenerateKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32]; // 256 bits
                rng.GetBytes(key);
                return key; // Return the generated key
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
                command.Parameters.AddWithValue("@createdAt", DateTime.UtcNow); // Store creation time
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        public string Encrypt(string plainText, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv; // Set the IV provided

                using (var msEncrypt = new MemoryStream())
                {
                    // Write the IV to the beginning of the MemoryStream
                    msEncrypt.Write(iv, 0, iv.Length);

                    using (var csEncrypt = new CryptoStream(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText); // Write the plaintext to be encrypted
                    }

                    // Return the full cipher text as a Base64 string
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }


        public string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);
            using (var aes = Aes.Create())
            {
                if (fullCipher.Length < 16) // Check for minimum length
                    throw new Exception("Cipher text is too short.");

                // The first 16 bytes are the IV
                var iv = new byte[16];
                Array.Copy(fullCipher, iv, iv.Length);
                aes.IV = iv;

                // The remaining bytes are the actual cipher text
                var cipher = new byte[fullCipher.Length - iv.Length];
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                // Retrieve the latest key
                var (key, retrievedIv) = RetrieveLatestKey();
                if (key == null || retrievedIv == null)
                {
                    throw new Exception("No encryption key or IV found for decryption.");
                }
                aes.Key = key;

                // Decrypt the data
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
                        return (key, iv); // Return the latest key and IV
                    }
                }
            }
            CloseConnection();
            return (null, null); // Return null if no keys found
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
                        return (key, iv); // Return the latest key and IV
                    }
                }
            }
            CloseConnection();
            return default; // Return null if no keys found
        }

        public void RotateEncryptionKey()
        {
            GenerateAndStoreNewKey(); // Generate and store a new key and IV
        }
    }
}
