using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace TuyaAPI.Services
{
    public class ControlLights
    {

        private readonly TuyaApiService _tuyaApiService = TuyaApiService.GetInstance();

        public async Task<HttpStatusCode> ControlDevice(string deviceId, string action, string value_name, string new_state)
        {
            var url = $"{_tuyaApiService.GetUrl()}skill";
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
                    accessToken = _tuyaApiService.AccesToken,
                    devId = deviceId,
                    value_name = new_state,
                }
            };
            using var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _tuyaApiService.GetHttpClient().PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to control device: {response.StatusCode}");
            }
            return response.StatusCode;
        }

        public async Task ToggleDeviceAsync(string deviceId, bool currentState)
        {
            var response = await ControlDevice(deviceId, "turnOnOff", "value", "0");
        }

        public async Task ChangeBrightnessAsync(string deviceId, int newBrightness)
        {
            string brightnessValue = (newBrightness * 10).ToString();
            var response = await ControlDevice(deviceId, "brightnessSet", "value", brightnessValue);
        }

        public async Task ChangeColorTemperatureAsync(string deviceId, int newTemperature)
        {
            int mappedTemperature = (int)((newTemperature - 1000) * 4.033) + 1000;
            string tempValue = mappedTemperature.ToString();
            var response = await ControlDevice(deviceId, "colorTemperatureSet", "value", tempValue);
        }
    }
}
