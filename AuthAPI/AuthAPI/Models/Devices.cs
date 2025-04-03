using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models
{
    public abstract class Device
    {
        [Key]
        public int Id { get; set; }

        public string CreationTime { get; set; }
        public string Platform { get; set; }

        // Külső kulcs az IdentityUser-hez
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Kapcsolat az entitásokkal
        public List<Entity> Entities { get; set; } = new();
    }
}
