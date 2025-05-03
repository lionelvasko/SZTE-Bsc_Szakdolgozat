using SomfyAPI.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace SomfyAPI.Services
{
    public class ShutterControl(SomfyApiService somfyApiService)
    {
        private readonly SomfyApiService _somfyApiService = somfyApiService;
        private static readonly Dictionary<string, TahomaExecutionId> _tahomaExecutionIds = [];

        public async Task OpenShutter(string deviceUrl)
        {
            await SendCommand("open", deviceUrl);
        }

        public async Task CloseShutter(string deviceUrl)
        {
            await SendCommand("close", deviceUrl);
        }

        public async Task StopShutter(string deviceUrl)
        {
            await SendCommand("stop", deviceUrl);
        }

        public async Task MyPositionShutter(string deviceUrl)
        {
            await SendCommand("my", deviceUrl);
        }

        public async Task SendCommand(string command, string deviceUrl)
        {
            if (_tahomaExecutionIds.TryGetValue(deviceUrl, out TahomaExecutionId? value))
            {
                await CancelSpecificExecution(value);
            }
            var payload = new
            {
                label = "Shutter",
                actions = new[]
                {
                        new
                        {
                            deviceURL = deviceUrl,
                            commands = new[]
                            {
                                new { name = command, parameters = GetParameters(command) }
                            }
                        }
                    }
            };

            var jsonContent = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _somfyApiService.SendRequest("POST", $"{_somfyApiService.BaseUrl}/exec/apply", content);

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine(response.StatusCode);
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception($"Error sending {command} command: {response.StatusCode} Given payload: {jsonContent}");
            }
            _tahomaExecutionIds[deviceUrl] = JsonSerializer.Deserialize<TahomaExecutionId>(await response.Content.ReadAsStringAsync())
                ?? throw new InvalidOperationException("Deserialization of TahomaExecutionId returned null.");
        }

        private static string[] GetParameters(string command)
        {
            var CommandConsts = new List<Command>
            {
                new() { CommandName = "close", Nparams = 1 },
                new() { CommandName = "down", Nparams = 1 },
                new() { CommandName = "identify", Nparams = 0 },
                new() { CommandName = "my", Nparams = 1 },
                new() { CommandName = "open", Nparams = 1 },
                new() { CommandName = "rest", Nparams = 1 },
                new() { CommandName = "stop", Nparams = 1 },
                new() { CommandName = "test", Nparams = 0 },
                new() { CommandName = "up", Nparams = 1 },
                new() { CommandName = "openConfiguration", Nparams = 1 }
            };

            return new string[CommandConsts.First(c => c.CommandName == command).Nparams];
        }

        private async Task CancelSpecificExecution(TahomaExecutionId execID)
        {
            var response = await _somfyApiService.GetHttpClient().DeleteAsync($"{_somfyApiService.BaseUrl}/exec/current/setup/{execID.Id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error cancelling execution {execID.Id}: {response.StatusCode}");
            }
        }
    }
}