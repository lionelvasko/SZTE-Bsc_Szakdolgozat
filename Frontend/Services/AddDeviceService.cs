using SomfyAPI.Services;
using System.Diagnostics;
using System.Net;
using Szakdoga.Models;
using Szakdoga.Requests;
using TuyaAPI.Services;

namespace Szakdoga.Services
{
    internal static class AddDeviceService
    {
        internal static async Task<Models.Device> AddSomfyDeviceAsync(string addedName)
        {
            try
            {
                var SingletonSomfyApiService = SomfyApiService.GetInstance();

                var generatedJson = await SingletonSomfyApiService.GetSetupJsonAsync();
                Debug.WriteLine(generatedJson);
                var mainDevice = SomfyAPI.Services.JsonHelper.GetDeviceFromJson(generatedJson);
                var newEntities = SomfyAPI.Services.JsonHelper.GetEntitiesFromJson(generatedJson);
                var convertedDevice = EntityDeviceConverter.ConvertToDevice(mainDevice, addedName, SingletonSomfyApiService);
                foreach (var entity in newEntities)
                {
                    var convertedEntity = EntityDeviceConverter.ConvertToEntity(entity, SingletonSomfyApiService);
                    convertedDevice.SomfyEntities.Add(convertedEntity as SomfyEntity);
                }
                Debug.WriteLine(convertedDevice);
                return convertedDevice;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in AddSomfyDeviceAsync: {ex.Message}");
                Debug.WriteLine($"Error in AddSomfyDeviceAsync: {ex.Message}");
                throw;
            }
        }

        internal static async Task<HttpResponseMessage> GenerateSomfyTokens(string email, string password, string homeUrl)
        {
            var SingletonSomfyApiService = SomfyApiService.GetInstance();
            SingletonSomfyApiService.SetOnlineUrl(homeUrl);
            bool validLogin = await SingletonSomfyApiService.LoginAsync(email, password);
            if (!validLogin)
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("Login failed") };
            }
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("Login successful") };
        }

        internal static async Task<Models.Device> AddTuyaDeviceAsync(string addedName)
        {
            try
            {
                var SingletonTuyaApiService = TuyaApiService.GetInstance();
                var resultDevices = await SingletonTuyaApiService.GetEntities();
                Debug.WriteLine(resultDevices);
                if (resultDevices == null || !resultDevices.Any())
                {
                    throw new Exception("No devices found");
                }

                var toAddEntites = TuyaAPI.Services.JsonHelper.GetEntitiesFromJson(resultDevices);

                var convertedDevice = EntityDeviceConverter.ConvertToDevice(toAddEntites.First(), addedName, SingletonTuyaApiService);

                foreach (var entity in toAddEntites)
                {
                    var convertedEntity = (Szakdoga.Models.Entity)EntityDeviceConverter.ConvertToEntity(entity, SingletonTuyaApiService);
                    convertedDevice.TuyaEntities.Add(convertedEntity as TuyaEntity);
                }
                Debug.WriteLine(convertedDevice);
                return convertedDevice;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in AddTuyaDeviceAsync: {ex.Message}");
                Debug.WriteLine($"Error in AddTuyaDeviceAsync: {ex.Message}");
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
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("Login successful") };
        }

    }
}

