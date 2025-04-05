using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
