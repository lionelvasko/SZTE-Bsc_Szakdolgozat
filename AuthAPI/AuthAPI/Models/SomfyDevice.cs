using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAPI.Models
{
    public class SomfyDevice : Device
    {
        public string Id { get; set; }
        public string GatewayId { get; set; }

    }
}
