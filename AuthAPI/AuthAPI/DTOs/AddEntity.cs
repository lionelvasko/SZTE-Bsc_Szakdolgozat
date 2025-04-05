using System.ComponentModel.DataAnnotations;

namespace AuthAPI.DTOs
{
    public class AddEntity
    {
        [Required]
        public string URL { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Platform { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

    }
}
