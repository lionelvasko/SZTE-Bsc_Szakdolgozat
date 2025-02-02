using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    public class State
    {
        public string Name { get; set; }
        public int? Type { get; set; } // Type is optional for some states
        public object Value { get; set; } // Can be different types depending on the state
    }
}
