using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    internal class TuyaEntity : Entity
    {
        internal string Brightness { get; set; }
        internal string ColorMode { get; set; }
        internal bool Online { get; set; }
        internal string State { get; set; }
        internal int ColorTemp { get; set; }
    }
}
