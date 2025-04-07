using System.Diagnostics;
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

        private string region;
        private string _accessToken;
        private string _refreshToken;

        private string registeredUsername;

        private TuyaApiService()
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

        public string GetAccessToken()
        {
            return _accessToken;
        }

        public string GetRefreshToken()
        {
            return _refreshToken;
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }

        public string GetRegion()
        {
            return region;
        }

        public void SetUrl(string countryCode)
        {
            region = countryCode;
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

        public string GetRegisteredUsername()
        {
            return registeredUsername;
        }

        public void SetTokens(string accessToken, string refreshToken)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
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
            Debug.WriteLine("Login URL: " + $"{_baseUrl}/homeassistant/auth.do");
            var response = await _httpClient.PostAsync($"{_baseUrl}auth.do", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Login response: " + responseContent);
                var loginResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                _accessToken = loginResponse.GetProperty("access_token").GetString();
                _refreshToken = loginResponse.GetProperty("refresh_token").GetString();

                registeredUsername = username;
            }
            else
            {
                Debug.WriteLine("Login failed: " + response.StatusCode);
            }
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> RefreshToken()
        {
            var refreshBody = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", _refreshToken },
                { "rand", RandomNumberGenerator.GetInt32(0, 2).ToString() }
            };
            using var content = new FormUrlEncodedContent(refreshBody);
            var response = await _httpClient.PostAsync($"{_baseUrl}access.do", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var refreshResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                _accessToken = refreshResponse.GetProperty("access_token").GetString();
                _refreshToken = refreshResponse.GetProperty("refresh_token").GetString();
            }
            return response.StatusCode;
        }

        public async Task<string> GetEntities(bool refreshAccessToken = false)
        {
            if (refreshAccessToken)
            {
                await RefreshToken();
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
                    accessToken = _accessToken
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
