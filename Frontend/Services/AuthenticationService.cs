using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Szakdoga.Models;

namespace Szakdoga.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public readonly static string JWT_AUTH_TOKEN = "jwt_auth_token";
        public readonly static string REMEMBER_ME_KEY = "remember_me";

        public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<HttpResponseMessage> LoginAsync(string email, string password, bool rememberMe)
        {
            var loginData = new
            {
                Email = email,
                Password = password
            };

            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5223/api/login", content);
            if (!response.IsSuccessStatusCode)
                return response;

            string token = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await SecureStorage.SetAsync(REMEMBER_ME_KEY, rememberMe.ToString());
            await SecureStorage.SetAsync(JWT_AUTH_TOKEN, token);

            ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyAuthenticationStateChanged();

            response.Content.Dispose();
            return response;
        }

        public async Task<HttpResponseMessage> RegisterAsync(RegisterModel registerModel)
        {
            try
            {
                var registerData = new
                {
                    Email = registerModel.Email,
                    Password = registerModel.Password,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName
                };

                var content = new StringContent(JsonSerializer.Serialize(registerData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:5223/api/register", content);
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.Unauthorized, Content = new StringContent(ex.Message) };
            }
        }

        public void Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            SecureStorage.Default.Remove(JWT_AUTH_TOKEN);
            SecureStorage.Default.Remove(REMEMBER_ME_KEY);
            SecureStorage.Default.Remove("tuya_token");
            SecureStorage.Default.Remove("tuya_refresh_token");
            SecureStorage.Default.Remove("tuya_region");
            SecureStorage.Default.Remove("somfy_token");
            SecureStorage.Default.Remove("somfy_url");
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyAuthenticationStateChanged();
        }
    }
}
