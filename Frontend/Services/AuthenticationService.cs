using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;
using Szakdoga.Requests;


namespace Szakdoga.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly DbService _dbService;
        public readonly static string JWT_AUTH_TOKEN = "jwt_auth_token";
        public readonly static string REMEMBER_ME_KEY = "remember_me";
        private ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity());

        public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, DbService dbServcice)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _dbService = dbServcice;
        }

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

            response.Content.Dispose();
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
