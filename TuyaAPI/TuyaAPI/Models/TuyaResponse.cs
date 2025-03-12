using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TuyaAPI.Models
{
    public class TuyaResponse
    {
        [JsonPropertyName("payload")]
        public Payload Payload { get; set; }

        [JsonPropertyName("header")]
        public Header Header { get; set; }
    }
}
