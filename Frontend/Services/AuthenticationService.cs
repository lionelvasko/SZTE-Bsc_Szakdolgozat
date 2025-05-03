using Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Claims;


namespace Szakdoga.Services
{
    public class AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, DbService dbServcice)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
        private readonly DbService _dbService = dbServcice;
        public readonly static string JWT_AUTH_TOKEN = "jwt_auth_token";
        public readonly static string REMEMBER_ME_KEY = "remember_me";

        public async Task<HttpResponseMessage> LoginAsync(LoginRequest request, bool rememberMe)
        {
            var response = await _dbService.Login(request);
            if (!response.IsSuccessStatusCode)
                return response;

            string token = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await SecureStorage.SetAsync(REMEMBER_ME_KEY, rememberMe.ToString());
            await SecureStorage.SetAsync(JWT_AUTH_TOKEN, token);

            ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyAuthenticationStateChanged();

            // Explicitly dispose of the response content to avoid issues with trimming and AOT compatibility
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                response.Content.Dispose();
            }

            return response;
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
