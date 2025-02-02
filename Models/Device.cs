using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    public class Device
    {
        public string DeviceURL { get; set; }
        public string Label { get; set; }
        public int Type { get; set; }
        public bool Available { get; set; }
    }
}
