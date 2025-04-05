using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAPI.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DeviceId { get; set; }
        public string URL { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

}
