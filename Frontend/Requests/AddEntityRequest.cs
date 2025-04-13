using System.ComponentModel.DataAnnotations;

namespace Szakdoga.Requests
{
    public class AddEntityRequest
    {
        [Required]
        public string URL { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Icon { get; set; }

        // Tuya-specific
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

        // Somfy-specific
        public string BaseUrl { get; set; } = string.Empty;
        public string CloudPasswordHashed { get; set; } = string.Empty;
        public string CloudUsername { get; set; } = string.Empty;
    }

}
