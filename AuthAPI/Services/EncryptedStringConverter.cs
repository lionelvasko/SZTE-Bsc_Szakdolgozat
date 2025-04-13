using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthAPI.Services
{
    public class EncryptedStringConverter : ValueConverter<string, string>
    {
        public EncryptedStringConverter(EncryptService encryptService)
            : base(v => encryptService.Encrypt(v),
                  v => encryptService.Decrypt(v))
        { }
    }
}
