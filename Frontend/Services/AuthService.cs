using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Services
{
    internal class AuthService
    {
        private readonly HttpClient _httpClient;


        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Login(string email, string password)
        {

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5240/auth/login", new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            await SecureStorage.SetAsync("auth_token", result.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
            return result.Token;
        }
        public async Task<string> Register(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5240/auth/register", new { Email = email, Password = password });
            if (!response.IsSuccessStatusCode)
                return null;
            return await Login(email, password);
        }

        public async Task Logout()
        {
            SecureStorage.Remove("auth_token");
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
    }
}
