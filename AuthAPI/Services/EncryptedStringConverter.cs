using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthAPI.Services
{
    /// <summary>
    /// A value converter that encrypts and decrypts string values using the provided <see cref="EncryptService"/>.
    /// </summary>
    public class EncryptedStringConverter : ValueConverter<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedStringConverter"/> class.
        /// </summary>
        /// <param name="encryptService">The encryption service used to encrypt and decrypt string values.</param>
        public EncryptedStringConverter(EncryptService encryptService)
            : base(v => encryptService.Encrypt(v),
                  v => encryptService.Decrypt(v))
        { }
    }
}
