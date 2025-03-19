using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string SomfyToken { get; set; } = string.Empty;
        public string SomfyUrl { get; set; } = string.Empty;
        public string TuyaToken { get; set; } = string.Empty;
        public string TuyaRefreshToken { get; set; } = string.Empty;
        public string TuyaRegion { get; set; } = string.Empty;

    }
}
