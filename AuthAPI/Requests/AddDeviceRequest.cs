using Microsoft.Build.Framework;

namespace AuthAPI.Requests
{
    public class AddDeviceRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Platform { get; set; } = string.Empty;
    }
}
