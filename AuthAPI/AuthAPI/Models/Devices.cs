using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        public string CreationTime { get; set; }
        public string Platform { get; set; }

        public string UserId { get; set; }

        public List<Entity> Entities { get; set; } = new();

        public override string ToString()
        {
            return $"{Id} - {CreationTime} - {Platform} - {UserId}";
        }
    }
}
