using System.Text.Json.Serialization;

namespace AuthAPI.DTOs
{
    public class TuyaEntityDTO : EntityDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Region { get; set; }
    }
}
