using SomfyAPI.Services;
using System.Net;
using TuyaAPI.Services;

namespace Szakdoga.Services
{
    internal static class DeviceService
    {
        internal static async Task<Models.Device> AddSomfyDeviceAsync()
        {
            try
            {
                var SingletonSomfyApiService = SomfyApiService.GetInstance();

                var generatedJson = await SingletonSomfyApiService.GetSetupJsonAsync();
                var mainDevice = SomfyAPI.Services.JsonHelper.GetDeviceFromJson(generatedJson);
                var newEntities = SomfyAPI.Services.JsonHelper.GetEntitiesFromJson(generatedJson);
                var convertedDevice = EntityDeviceConverter.ConvertToDevice(mainDevice);
                convertedDevice.Email = SingletonSomfyApiService.GetRegisteredUsername();

                foreach (var entity in newEntities)
                {
                    var convertedEntity = (Szakdoga.Models.Entity)EntityDeviceConverter.ConvertToEntity(entity);
                    convertedDevice.Entities.Add(convertedEntity);
                }
                return convertedDevice;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in AddSomfyDeviceAsync: {ex.Message}");
                throw;
            }
        }

        internal static async Task<HttpResponseMessage> GenerateSomfyTokens(string email, string password, string homeUrl)
        {
            var SingletonSomfyApiService = SomfyApiService.GetInstance();
            bool validLogin = await SingletonSomfyApiService.LoginAsync(email, password, homeUrl);
            if (!validLogin)
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("Login failed") };
            }
            //await SetTokensSS(true);
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("Login successful") };
        }

        internal static async Task<Models.Device> AddTuyaDeviceAsync()
        {
            try
            {
                var SingletonTuyaApiService = TuyaApiService.GetInstance();
                var resultDevices = await SingletonTuyaApiService.GetEntities();
                if (resultDevices == null || !resultDevices.Any())
                {
                    throw new Exception("No devices found");
                }

                var toAddEntites = TuyaAPI.Services.JsonHelper.GetEntitiesFromJson(resultDevices);

                var convertedDevice = EntityDeviceConverter.ConvertToDevice(toAddEntites.First());
                convertedDevice.Email = SingletonTuyaApiService.GetRegisteredUsername();

                foreach (var entity in toAddEntites)
                {
                    var convertedEntity = (Szakdoga.Models.Entity)EntityDeviceConverter.ConvertToEntity(entity);
                    convertedDevice.Entities.Add(convertedEntity);
                }
                return convertedDevice;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in AddTuyaDeviceAsync: {ex.Message}");
                throw;
            }
        }

        internal static async Task<HttpResponseMessage> GenerateTuyaTokens(string username, string password, string region)
        {
            var SingletonTuyaApiService = TuyaApiService.GetInstance();
            SingletonTuyaApiService.SetUrl(region);

            var response = await SingletonTuyaApiService.Login(username, password, region);
            if (response == null)
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("Login failed") };
            }
            await SetTokensSS(false);
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("Login successful") };
        }

        internal static async Task SetTokensSS(bool somfyOrTuya)
        {
            if(somfyOrTuya)
            {
                var SingletonSomfyApiService = SomfyApiService.GetInstance();
                await SecureStorage.Default.SetAsync("somfy_token", SingletonSomfyApiService.GetAccessToken());
                await SecureStorage.Default.SetAsync("somfy_url", SingletonSomfyApiService.GetURL());
            }
            else
            {
                var SingletonTuyaApiService = TuyaApiService.GetInstance();
                await SecureStorage.Default.SetAsync("tuya_token", SingletonTuyaApiService.GetAccessToken());
                await SecureStorage.Default.SetAsync("tuya_refresh_token", SingletonTuyaApiService.GetRefreshToken());
                await SecureStorage.Default.SetAsync("tuya_region", SingletonTuyaApiService.GetRegion());
            }
        }

        internal static void UpdateApiServiceTokensTuya(string accessToken, string refreshToken, string region)
        {
            var SingletonTuyaApiService = TuyaApiService.GetInstance();
            SingletonTuyaApiService.SetTokens(accessToken, refreshToken);
        }

        internal static void UpdateApiServiceTokensSomfy(string token, string url)
        {
            var SingletonSomfyApiService = SomfyApiService.GetInstance();
            SingletonSomfyApiService.SetToken(token);
        }

        internal static async Task SaveTokensToUser()
        {
            var AuthenticationService = new AuthenticationService(new HttpClient(), new CustomAuthenticationStateProvider(), new UserInfoService());
        }

    }
}

