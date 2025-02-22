namespace Szakdoga.Services
{
    public class SecureStorageService
    {
        public async Task SaveTokenAsync(string token)
        {
            await SecureStorage.SetAsync("auth_token", token);
        }

        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.GetAsync("auth_token");
        }

        public async Task ClearTokenAsync()
        {
            SecureStorage.Remove("auth_token");
        }
    }

}
