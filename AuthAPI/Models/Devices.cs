using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models
{
    public class Device
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CreationTime { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string UserId { get; set; }
    }

}
