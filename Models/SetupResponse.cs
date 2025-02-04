using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    public class SetupResponse
    {
        [JsonPropertyName("gateways")]
        public List<Device> Devices { get; set; }
        [JsonPropertyName("devices")]
        public List<Entity> Entities { get; set; }
    }
}
