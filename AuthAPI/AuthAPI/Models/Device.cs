using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAPI.Models
{
    public abstract class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        [Required]
        public string Platform { get; set; }

        [Required]
        public string UserId { get; set; }  // Felhasználó azonosítója

        public List<Entity> Entities { get; set; } = new List<Entity>();
    }
}
