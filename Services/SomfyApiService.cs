using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Szakdoga.Models;

namespace Szakdoga.Services
{
    public class SomfyApiService
    {
        private static SomfyApiService _instance;
        private readonly HttpClient _httpClient;
        private string _baseUrl;
        private string _gatewayPin;
        private string _sessionId;
        private string _token;

        private SomfyApiService()
        {
            if (_httpClient == null)
            {
                _httpClient = CreateHttpClientWithCertificate(); // Initialize the HttpClient with the cert
            }
        }

        // Singleton instance getter
        public static SomfyApiService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SomfyApiService(); // Initialize the singleton instance
            }
            return _instance;
        }

        // Load the certificate from the resources folder
        private static HttpClient CreateHttpClientWithCertificate()
        {
            var handler = new HttpClientHandler();

            // Load the certificate file into X509Certificate2
            using (var certStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Szakdoga.Resources.overkiz-root-ca-2048.crt"))
            {
                if (certStream == null)
                {
                    throw new InvalidOperationException("Certificate resource not found. LIO");
                }

                using (var memoryStream = new MemoryStream())
                {
                    certStream.CopyTo(memoryStream);
                    var certificate = new X509Certificate2(memoryStream.ToArray());
                    handler.ClientCertificates.Add(certificate);
                }
            }

            // Ignore SSL policy errors (to work with self-signed certificates)
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;

                //You can also inspect the certificate chain here if needed
                return cert != null && cert.Equals(handler.ClientCertificates[0]);
            };

            return new HttpClient(handler);
        }


        public async Task<bool> LoginAsync(string email, string password, string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("Base URL is not set.");
            }
            this._baseUrl = baseUrl;

            var loginUrl = $"{_baseUrl}/enduser-mobile-web/enduserAPI/login";
            var loginBody = new Dictionary<string, string>
            {
                { "userId", email },
                { "userPassword", password }
            };

            using var content = new FormUrlEncodedContent(loginBody);
            using var response = await _httpClient.PostAsync(loginUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var cookies = response.Headers.GetValues("Set-Cookie");
                var sessionCookie = cookies.FirstOrDefault(c => c.Contains("JSESSIONID"));

                if (sessionCookie != null)
                {
                    _sessionId = sessionCookie.Split(';')[0].Split('=')[1]; // Extract JSESSIONID value
                }

                return true;
            }

            return false;
        }

        public async Task<string> GenerateTokenAsync(string _gatewayPin)
        {
            if (string.IsNullOrEmpty(_gatewayPin))
            {
                throw new InvalidOperationException("Gateway Pin is not set.");
            }
            this._gatewayPin = _gatewayPin;
            var tokenUrl = $"{_baseUrl}/enduser-mobile-web/enduserAPI/config/{_gatewayPin}/local/tokens/generate";
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"JSESSIONID={_sessionId}");

            var response = await _httpClient.GetAsync(tokenUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<JsonElement>(responseBody);
                return tokenData.GetProperty("token").GetString();
            }
            return null;
        }

        public async Task<bool> ActivateTokenAsync(string token)
        {
            var activateTokenUrl = $"{_baseUrl}/enduser-mobile-web/enduserAPI/config/{_gatewayPin}/local/tokens";
            var activateBody = new
            {
                label = "My MAUI Blazor Token",
                token = token,
                scope = "devmode"
            };

            var response = await _httpClient.PostAsJsonAsync(activateTokenUrl, activateBody);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Entity>> GetEntitiesAsync()
        {
            var entities = new List<Entity>();

            var setupUrl = $"{_baseUrl}/enduser-mobile-web/enduserAPI/setup";
            //var setupUrl = $"https://gateway-{_gatewayPin}:{8443}/enduser-mobile-web/1/enduserAPl"; //Nem működik egy update miatt a local hívás 😓
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _httpClient.GetAsync(setupUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseBody);
                var setupData = JsonSerializer.Deserialize<SetupResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (setupData?.Devices != null)
                {
                    foreach (var device in setupData.Devices)
                    {
                        entities.Add(new Entity
                        {
                            DeviceURL = device.DeviceURL,
                            Label = device.Label,
                            Type = device.Type,
                            Available = device.Available
                        });
                    }
                }
            }
            else
            {
                throw new HttpRequestException($"Failed to fetch setup data: {response.StatusCode}");
            }

            return entities;
        }
    }
}
