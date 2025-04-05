using System.ComponentModel.DataAnnotations;

namespace AuthAPI.DTOs
{
    public class AddEntity
    {
        public string URL { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Icon { get; set; }

        // Tuya-specific fields
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

        // Somfy-specific fields
        public string BaseUrl { get; set; } = string.Empty;
        public string GatewayPin { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

}
