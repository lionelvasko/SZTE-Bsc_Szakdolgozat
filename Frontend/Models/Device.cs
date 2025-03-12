using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    internal class Device
    {
        internal string Id { get; set; }
        internal string GatewayId { get; set; }

        internal string Platform { get; set; }
        internal string Email { get; set; }

        internal List<Entity> Entities { get; set; }
    }
}
