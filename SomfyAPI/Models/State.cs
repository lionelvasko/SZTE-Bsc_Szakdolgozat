using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomfyAPI.Models
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // Type is optional for some states
    }
}
