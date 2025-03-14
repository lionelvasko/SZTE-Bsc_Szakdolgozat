using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TuyaAPI.Services
{
    public static class ControlLights
    {

        private static readonly TuyaApiService _tuyaApiService = TuyaApiService.GetInstance();
        private static readonly HttpClient _httpClient = _tuyaApiService.GetHttpClient();
        private static readonly string _baseUrl = _tuyaApiService.GetUrl();
        private static readonly string _accessToken = _tuyaApiService.GetAccessToken();

        public static async Task<HttpStatusCode> ControlDevice(string deviceId, string action, string value_name, string new_state)
        {
            var url = $"{_baseUrl}skill";
            var requestBody = new
            {
                header = new
                {
                    name = action,
                    @namespace = "control",
                    payloadVersion = 1
                },
                payload = new
                {
                    accessToken = _accessToken,
                    devId = deviceId,
                    value_name = new_state,
                }
            };
            using var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to control device: {response.StatusCode}");
            }
            return response.StatusCode;
        }

        public static async Task ToggleDeviceAsync(string deviceId, bool currentState)
        {
            string newState = (!currentState) ? "1" : "0";
            var response = await ControlDevice(deviceId, "turnOnOff", "value", newState);

            if (response == HttpStatusCode.OK)
            {
                // Frissítési logika a memóriában vagy UI-ban
                Console.WriteLine($"Device {deviceId} state changed to {newState}");
            }
        }

        public static async Task ChangeBrightnessAsync(string deviceId, int newBrightness)
        {
            string brightnessValue = (newBrightness * 10).ToString();
            var response = await ControlDevice(deviceId, "brightnessSet", "value", brightnessValue);

            if (response == HttpStatusCode.OK)
            {
                // Frissítési logika a memóriában vagy UI-ban
                Console.WriteLine($"Device {deviceId} brightness set to {newBrightness}");
            }
        }

        public static async Task ChangeColorTemperatureAsync(string deviceId, int newTemperature)
        {
            // min temp = 1000, reports as 1000
            // max temp = 10000, reports as 36294
            int mappedTemperature = (int)((newTemperature - 1000) * 4.033) + 1000;
            string tempValue = mappedTemperature.ToString();
            var response = await ControlDevice(deviceId, "colorTemperatureSet", "value", tempValue);

            if (response == HttpStatusCode.OK)
            {
                // Frissítési logika a memóriában vagy UI-ban
                Console.WriteLine($"Device {deviceId} color temperature set to {mappedTemperature}");
            }
        }
    }
}
