using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TuyaAPI.Models
{
    public class Header
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("payloadVersion")]
        public int PayloadVersion { get; set; }
    }
}
