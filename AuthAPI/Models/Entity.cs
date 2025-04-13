using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid DeviceId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Icon { get; set; }
    }

}
