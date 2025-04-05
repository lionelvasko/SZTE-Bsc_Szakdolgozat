using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CreationTime { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string UserId { get; set; }
        public ICollection<Entity> Entities { get; set; } = new List<Entity>();
    }

}
