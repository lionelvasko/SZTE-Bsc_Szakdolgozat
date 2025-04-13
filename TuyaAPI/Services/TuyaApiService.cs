using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TuyaAPI.Services
{
    public class TuyaApiService
    {
        private static TuyaApiService? _instance;
        private HttpClient _httpClient;

        private string _baseUrl = "https://px1.tuyaeu.com/homeassistant/";
        private readonly string PLATFORM = "tuya";

        public string Region { get; set; }
        public string AccesToken { get; set; }
        public string RefreshToken { get; set; }

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
            if (_instance is null)
            {
                _instance = new TuyaApiService();
            }
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
                _baseUrl.Replace("eu", "us");
            }
            else if (countryCode == "44")
            {
                _baseUrl.Replace("eu", "eu");
            }
            else
            {
                _baseUrl.Replace("eu", "cn");
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
                AccesToken = loginResponse.GetProperty("access_token").GetString();
                RefreshToken = loginResponse.GetProperty("refresh_token").GetString();
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
                AccesToken = refreshResponse.GetProperty("access_token").GetString();
                RefreshToken = refreshResponse.GetProperty("refresh_token").GetString();
            }
            return response.StatusCode;
        }

        public async Task<string> GetEntities(bool refreshAccessToken = false)
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
