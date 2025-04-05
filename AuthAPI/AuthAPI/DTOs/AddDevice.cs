using AuthAPI.Models;
using Microsoft.Build.Framework;

namespace AuthAPI.DTOs
{
    public class AddDevice
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Platform { get; set; } = string.Empty;
        public List<AddEntity> Entities { get; set; } = new List<AddEntity>();
    }
}
