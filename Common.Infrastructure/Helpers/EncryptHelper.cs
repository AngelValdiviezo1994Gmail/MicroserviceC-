using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Helpers
{
    public class EncryptHelper : IEncryptHelper
    {
        private const string PrivateKeyHashFactory = "zPBpJkfU24uQ1zc1n63FsDNQ2Inc4hGN";

        /// <summary>
        /// Encrypt base SHA 256. It won't be decrypted.Encrypt with 2 parameters.
        /// </summary>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <returns>Secure Encrypted</returns>
        public string Encrypt(string input)
        {
            var hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(PrivateKeyHashFactory));

            // Computes the signature by hashing the salt with the secret key as the key
            var bytes = hashObject.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Base 64 Encode
            var builder = new StringBuilder();
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }
            return builder.ToString();
        }


        public string EncryptAES(string plainText)
        {
            if (plainText == null)
            {
                return null;
            }
            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(PrivateKeyHashFactory);

            // Hash the password with SHA256
            passwordBytes = SHA512.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = EncryptAes(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        public string DecryptAES(string encryptedText)
        {
            try
            {
                if (encryptedText == null)
                {
                    return null;
                }
                // Get the bytes of the string
                var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
                var passwordBytes = Encoding.UTF8.GetBytes(PrivateKeyHashFactory);

                passwordBytes = SHA512.Create().ComputeHash(passwordBytes);

                var bytesDecrypted = DecryptAes(bytesToBeDecrypted, passwordBytes);

                return Encoding.UTF8.GetString(bytesDecrypted);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static byte[] EncryptAes(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using var ms = new MemoryStream();
            using var aes = new RijndaelManaged();
            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            aes.Mode = CipherMode.CBC;

            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                cs.Close();
            }

            var encryptedBytes = ms.ToArray();

            return encryptedBytes;
        }

        private static byte[] DecryptAes(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using var ms = new MemoryStream();
            using var aes = new RijndaelManaged();
            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;

            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                cs.Close();
            }

            var decryptedBytes = ms.ToArray();

            return decryptedBytes;
        }
    }
}
