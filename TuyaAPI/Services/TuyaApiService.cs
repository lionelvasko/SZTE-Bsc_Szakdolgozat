using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TuyaAPI.Services
{
    public class TuyaApiService
    {
        private static TuyaApiService? _instance;
        private readonly HttpClient _httpClient;

        private string _baseUrl = "https://px1.tuyaeu.com/homeassistant/";
        private readonly string PLATFORM = "tuya";

        public string Region { get; set; } = string.Empty;
        public string AccesToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        public TuyaApiService()
        {
            if (_httpClient is null)
            {
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.ConnectionClose = true;
                _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                _httpClient.Timeout = TimeSpan.FromSeconds(5);
            }
        }

        public static TuyaApiService GetInstance()
        {
            _instance ??= new TuyaApiService();
            return _instance;
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }
        public void SetUrl(string countryCode)
        {
            Region = countryCode;
            if (countryCode == "1")
            {
                _baseUrl = _baseUrl.Replace("eu", "us");
            }
            else if (countryCode == "44")
            {
                _baseUrl = _baseUrl.Replace("eu", "eu");
            }
            else
            {
                _baseUrl = _baseUrl.Replace("eu", "cn");
            }
        }

        public string GetUrl()
        {
            return _baseUrl;
        }

        public async Task<HttpStatusCode> Login(string username, string password, string countryCode)
        {
            var loginBody = new Dictionary<string, string>
                {
                    { "userName", username },
                    { "password", password },
                    { "countryCode", countryCode},
                    {"bizType", PLATFORM },
                    {"from", PLATFORM }
                };

            using var content = new FormUrlEncodedContent(loginBody);
            var response = await _httpClient.PostAsync($"{_baseUrl}auth.do", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                if (loginResponse.TryGetProperty("access_token", out JsonElement accessTokenElement))
                {
                    AccesToken = accessTokenElement.GetString() ?? string.Empty;
                }

                if (loginResponse.TryGetProperty("refresh_token", out JsonElement refreshTokenElement))
                {
                    RefreshToken = refreshTokenElement.GetString() ?? string.Empty;
                }
            }
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> DoRefreshToken()
        {
            var refreshBody = new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", RefreshToken },
                    { "rand", RandomNumberGenerator.GetInt32(0, 2).ToString() }
                };
            using var content = new FormUrlEncodedContent(refreshBody);
            var response = await _httpClient.PostAsync($"{_baseUrl}access.do", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var refreshResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

                if (refreshResponse.TryGetProperty("access_token", out JsonElement accessTokenElement))
                {
                    AccesToken = accessTokenElement.GetString() ?? string.Empty;
                }

                if (refreshResponse.TryGetProperty("refresh_token", out JsonElement refreshTokenElement))
                {
                    RefreshToken = refreshTokenElement.GetString() ?? string.Empty;
                }
            }
            return response.StatusCode;
        }

        public async Task<string> GetEntities(bool refreshAccessToken = true)
        {
            if (refreshAccessToken)
            {
                await DoRefreshToken();
            }

            var url = $"{_baseUrl}skill";
            var requestBody = new
            {
                header = new
                {
                    name = "Discovery",
                    @namespace = "discovery",
                    payloadVersion = 1
                },
                payload = new
                {
                    accessToken = AccesToken,
                }
            };

            using var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch setup data: {response.StatusCode}");
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
