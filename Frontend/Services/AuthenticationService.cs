using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
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
        private readonly UserInfoService _userInfoService;
        public readonly static string JWT_AUTH_TOKEN = "jwt_auth_token";
        public readonly static string REMEMBER_ME_KEY = "remember_me";

        public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, UserInfoService userInfoService)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _userInfoService = userInfoService;
        }

        public async Task<HttpResponseMessage> LoginAsync(string email, string password, bool rememberMe)
        {
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

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await SecureStorage.SetAsync(REMEMBER_ME_KEY, rememberMe.ToString());
            await SecureStorage.SetAsync(JWT_AUTH_TOKEN, token);

            var user = DecodeJwtToken(token);
            _userInfoService.StroreName(user.FirstOrDefault(c => c.Type == "unique_name")?.Value);
            _userInfoService.StroreEmail(user.FirstOrDefault(c => c.Type == "email")?.Value);

            ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyAuthenticationStateChanged();

            return response;
        }

        public void Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            SecureStorage.Default.Remove(JWT_AUTH_TOKEN);
            SecureStorage.Default.Remove(REMEMBER_ME_KEY);
            SecureStorage.Default.Remove(UserInfoService.NAME_KEY);
            SecureStorage.Default.Remove(UserInfoService.EMAIL_KEY);
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyAuthenticationStateChanged();
        }

        public async Task SetTokens()
        {
            var tuyaAccesToken = await SecureStorage.GetAsync("tuya_token");
            var somfyAccesToken = await SecureStorage.GetAsync("somfy_token");
            var tuyaRefreshToken = await SecureStorage.GetAsync("tuya_refresh_token");

            var authToken = await SecureStorage.GetAsync(JWT_AUTH_TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);


            if (tuyaAccesToken != null)
            {
                var content = new StringContent(JsonSerializer.Serialize(new { AccessToken = tuyaAccesToken, RefreshToken = tuyaRefreshToken }), Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("http://localhost:5223/authapi/tokens/tuya", content);
            }
            else if (somfyAccesToken != null)
            {
                var content = new StringContent(JsonSerializer.Serialize(new { AccessToken = somfyAccesToken, RefreshToken = "" }), Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("http://localhost:5223/authapi/tokens/somfy", content);
            }
        }
        private IEnumerable<Claim> DecodeJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token) as JwtSecurityToken;

            return jwtToken?.Claims;
        }

    }
}
