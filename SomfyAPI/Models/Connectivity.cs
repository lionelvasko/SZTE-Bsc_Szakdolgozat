using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomfyAPI.Models
{
    public class Connectivity
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string ProtocolVersion { get; set; }
    }
}
