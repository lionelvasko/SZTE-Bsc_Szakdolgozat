using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Szakdoga.Models;

namespace Szakdoga.Services
{
    public class AuthenticationService
    {
        private bool _rememberMe;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly string JWT_AUTH_TOKEN = "jwt_auth_token";

        public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<HttpResponseMessage> LoginAsync(string email, string password, bool rememberMe)
        {
            _rememberMe = rememberMe;
            var loginData = new
            {
                Email = email,
                Password = password
            };

            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5223/authapi/login", content);

            if (!response.IsSuccessStatusCode)
                return response;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jwtResponse = JsonSerializer.Deserialize<LoginResponse>(jsonResponse);
            string token = jwtResponse.Token;

            await SecureStorage.Default.SetAsync(JWT_AUTH_TOKEN, token);
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyAuthenticationStateChanged();

            return response;
        }

        public void Logout()
        {
            SecureStorage.Default.Remove(JWT_AUTH_TOKEN);
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyAuthenticationStateChanged();
        }
    }
}
