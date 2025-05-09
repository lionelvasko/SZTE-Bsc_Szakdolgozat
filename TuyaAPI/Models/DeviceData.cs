﻿using System.Text.Json.Serialization;

namespace TuyaAPI.Models
{
    public class DeviceData
    {
        [JsonPropertyName("brightness")]
        public string? Brightness { get; set; }

        [JsonPropertyName("color_mode")]
        public string? ColorMode { get; set; }

        [JsonPropertyName("online")]
        public bool Online { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("color_temp")]
        public int ColorTemp { get; set; }
    }
}
