using System.Diagnostics;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace SomfyAPI.Services
{
    public class SomfyApiService
    {
        private static SomfyApiService? _instance;
        private readonly HttpClient _httpClient;
        public string BaseUrl { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public SomfyApiService()
        {
            if (_httpClient == null)
            {
                _httpClient = CreateHttpClientWithCertificate();
                _httpClient.DefaultRequestHeaders.ConnectionClose = true;
                _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                _httpClient.Timeout = TimeSpan.FromSeconds(5);
            }
        }

        public static SomfyApiService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SomfyApiService();
            }
            return _instance;
        }

        private static HttpClient CreateHttpClientWithCertificate()
        {
            var handler = new HttpClientHandler();
            using (var certStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SomfyAPI.Resources.overkiz-root-ca-2048.crt"))
            {
                if (certStream == null)
                {
                    throw new InvalidOperationException("Certificate resource not found.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    certStream.CopyTo(memoryStream);
                    var certificate = X509CertificateLoader.LoadCertificate(memoryStream.ToArray());
                    handler.ClientCertificates.Add(certificate);
                }
            }
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;

                return cert != null && cert.Equals(handler.ClientCertificates[0]);
            };
            return new HttpClient(handler);
        }

        public void SetOnlineUrl(string url)
        {
            BaseUrl = $"{url}/enduser-mobile-web/enduserAPI";
        }

        public string GetURL()
        {
            return BaseUrl;
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            CreateHttpClientWithCertificate();
            if (string.IsNullOrEmpty(BaseUrl))
            {
                throw new InvalidOperationException("Base URL is not set.");
            }
            var loginUrl = $"{BaseUrl}/login";
            var loginBody = new Dictionary<string, string>
                {
                    { "userId", email },
                    { "userPassword", password }
                };

            using var content = new FormUrlEncodedContent(loginBody);
            using var response = await _httpClient.PostAsync(loginUrl, content);

            if (response.IsSuccessStatusCode)
            {
                if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    var sessionCookie = cookies.FirstOrDefault(c => c.StartsWith("JSESSIONID"));
                    if (!string.IsNullOrEmpty(sessionCookie))
                    {
                        var sessionId = sessionCookie.Split(';')[0].Split('=')[1];
                        _httpClient.DefaultRequestHeaders.Remove("Cookie");
                        _httpClient.DefaultRequestHeaders.Add("Cookie", $"JSESSIONID={sessionId}");
                    }
                }
                Username = email;
                Password = password;
                return true;
            }

            return false;
        }

        public async Task<string> GetSetupJsonAsync()
        {
            string setupUrl = GetURL() + "/setup";
            var response = await _httpClient.GetAsync(setupUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch setup data: {response.StatusCode}");
            }
            Debug.WriteLine("Setup response: " + response);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> SendRequest(string type, string url, StringContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod(type), url)
            {
                Content = content
            };
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                throw new HttpRequestException($"Request failed: {response.StatusCode}");
            }
        }
    }
}
