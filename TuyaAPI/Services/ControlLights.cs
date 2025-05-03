using System.Net;
using System.Text;
using System.Text.Json;

namespace TuyaAPI.Services
{
    public class ControlLights
    {

        private readonly TuyaApiService _tuyaApiService = TuyaApiService.GetInstance();

        public async Task<HttpStatusCode> ControlDevice(string deviceId, string action, string new_state)
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
            string lightState = "1";
            if (currentState)
            {
                lightState = "0";
            }
            {
            }
            await ControlDevice(deviceId, "turnOnOff", lightState);
        }
    }
}
