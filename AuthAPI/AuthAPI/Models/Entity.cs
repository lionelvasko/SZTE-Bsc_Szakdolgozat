using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DeviceId { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Icon { get; set; }
    }

}
