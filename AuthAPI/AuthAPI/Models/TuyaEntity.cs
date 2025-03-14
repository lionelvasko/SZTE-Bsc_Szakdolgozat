using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAPI.Models
{
    public class TuyaEntity : Entity
    {
        public string Brightness { get; set; }
        public string ColorMode { get; set; }
        public bool Online { get; set; }
        public string State { get; set; }   
        public int ColorTemp { get; set; }
    }
}
