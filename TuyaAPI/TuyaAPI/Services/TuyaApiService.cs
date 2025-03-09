using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TuyaAPI.Models;

namespace TuyaAPI.Services
{
    public class TuyaApiService
    {
        private static TuyaApiService? _instance;
        private HttpClient _httpClient;
        private string _baseUrl;
        private string _clientId;

        private string _accessToken;
        private string _refreshToken;
        private string _secret;

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

        public void SetURL(string url)
        {
            _baseUrl = url;
        }

        public string GetURL()
        {
            return _baseUrl;
        }

        public void SetClientId(string clientId)
        {
            this._clientId = clientId;
        }

        public void SetSecret(string secret)
        {
            this._secret = secret;
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }

        private string GenerateTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }

        private string GenerateNonce()
        {
            return Guid.NewGuid().ToString();
        }

        private string GenerateSign(string timestamp, string nonce)
        {
            var sign = $"{_clientId}{timestamp}{nonce}{_secret}";
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(sign);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public async Task GetAccesTokenWithSimpleMode()
        {
            _httpClient.DefaultRequestHeaders.Add("client_id", _clientId);
            _httpClient.DefaultRequestHeaders.Add("sign_method", "HMAC-SHA256");
            _httpClient.DefaultRequestHeaders.Add("t", GenerateTimestamp());
            _httpClient.DefaultRequestHeaders.Add("nonce", GenerateNonce());
            _httpClient.DefaultRequestHeaders.Add("sign", GenerateSign(_httpClient.DefaultRequestHeaders.GetValues("t").First(), _httpClient.DefaultRequestHeaders.GetValues("nonce").First()));

            var response = await _httpClient.GetAsync(_baseUrl + "/v1.0/token?grant_type=1");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var token = JsonSerializer.Deserialize<SimpleModeToken>(responseContent);
                    this._accessToken = token.Result.Access_token;
                    this._refreshToken = token.Result.Refresh_token;
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to get access token", e);
                }
            }
            else
            {
                throw new Exception("Not Succesfull ");
            }

        }
    }
}
