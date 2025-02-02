using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    public class SetupResponse
    {
        public List<Gateway> Gateways { get; set; }
        public List<Device> Devices { get; set; }
    }
}
