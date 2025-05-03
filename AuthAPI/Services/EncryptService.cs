using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Services
{
    /// <summary>  
    /// Provides encryption and decryption services for string values.  
    /// </summary>  
    public class EncryptService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        /// <summary>  
        /// Initializes a new instance of the <see cref="EncryptService"/> class with the specified secret.  
        /// </summary>  
        /// <param name="secret">The secret key used for encryption and decryption.</param>  
        public EncryptService(string secret)
        {
            _key = SHA256.HashData(Encoding.UTF8.GetBytes(secret));
            _iv = [.. _key.Take(16)];
        }

        /// <summary>  
        /// Encrypts the specified plain text string.  
        /// </summary>  
        /// <param name="plainText">The plain text to encrypt.</param>  
        /// <returns>The encrypted string in Base64 format.</returns>  
        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>  
        /// Decrypts the specified encrypted string.  
        /// </summary>  
        /// <param name="cipherText">The encrypted string in Base64 format to decrypt.</param>  
        /// <returns>The decrypted plain text string.</returns>  
        public string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var decryptor = aes.CreateDecryptor();
            var cipherBytes = Convert.FromBase64String(cipherText);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
