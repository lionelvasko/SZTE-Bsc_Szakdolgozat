using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthAPI.Services
{
    /// <summary>
    /// A value converter that encrypts and decrypts string values using the provided <see cref="EncryptService"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EncryptedStringConverter"/> class.
    /// </remarks>
    /// <param name="encryptService">The encryption service used to encrypt and decrypt string values.</param>
    public class EncryptedStringConverter(EncryptService encryptService) : ValueConverter<string, string>(v => encryptService.Encrypt(v),
              v => encryptService.Decrypt(v))
    {
    }
}
