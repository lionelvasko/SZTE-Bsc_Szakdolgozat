using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Szakdoga.Models;

namespace Szakdoga.Services.Somfy
{
    internal static class ShutterControl
    {
        private static readonly HttpClient _httpClient = SomfyApiService.GetInstance().GetHttpClient();
        private static readonly string _tahomaApiUrl = SomfyApiService.GetInstance().GetURL();
        private static Dictionary<string, TahomaExecutionId> _tahomaExecutionIds = [];

        public static async Task OpenShutter(string deviceUrl)
        {
            await SendCommand("open", deviceUrl);
        }

        public static async Task CloseShutter(string deviceUrl)
        {
            await SendCommand("close", deviceUrl);
        }

        public static async Task StopShutter(string deviceUrl)
        {
            await SendCommand("stop", deviceUrl);
        }

        public static async Task MyPositionShutter(string deviceUrl)
        {
            await SendCommand("my", deviceUrl);
        }

        private static async Task SendCommand(string command, string deviceUrl)
        {
            if (_tahomaExecutionIds.ContainsKey(deviceUrl))
            {
                await CancelSpecificExecution(_tahomaExecutionIds[deviceUrl]);
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

            Console.WriteLine($"[{DateTime.Now}] Sending request to {_tahomaApiUrl}/exec/apply with payload: {jsonContent}");

            // For faster response times, we set the ConnectionClose header to true
            

            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.PostAsync($"{_tahomaApiUrl}/exec/apply", content);
            stopwatch.Stop();

            Console.WriteLine($"[{DateTime.Now}] Response received in {stopwatch.ElapsedMilliseconds} ms");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error sending {command} command: {response.StatusCode} Given payload: {jsonContent}");
            }
            _tahomaExecutionIds[deviceUrl] = JsonSerializer.Deserialize<TahomaExecutionId>(json: await response.Content.ReadAsStringAsync());
        }

        private static string[] GetParameters(string command)
        {
            var CommandConsts = new List<Models.Command>
                    {
                        new Models.Command { CommandName = "close", Nparams = 1 },
                        new Models.Command { CommandName = "down", Nparams = 1 },
                        new Models.Command { CommandName = "identify", Nparams = 0 },
                        new Models.Command { CommandName = "my", Nparams = 1 },
                        new Models.Command { CommandName = "open", Nparams = 1 },
                        new Models.Command { CommandName = "rest", Nparams = 1 },
                        new Models.Command { CommandName = "stop", Nparams = 1 },
                        new Models.Command { CommandName = "test", Nparams = 0 },
                        new Models.Command { CommandName = "up", Nparams = 1 },
                        new Models.Command { CommandName = "openConfiguration", Nparams = 1 }
                };

            return new string[CommandConsts.First(c => c.CommandName == command).Nparams];
        }

        private static async Task CancelSpecificExecution(TahomaExecutionId execID)
        {
            var response = await _httpClient.DeleteAsync($"{_tahomaApiUrl}/exec/current/setup/{execID.Id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error cancelling execution {execID.Id}: {response.StatusCode}");
            }
        }
    }
}
