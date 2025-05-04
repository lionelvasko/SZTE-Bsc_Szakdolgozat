using SomfyAPI.Services;
using System.Diagnostics;
using System.Net;
using Szakdoga.Models;
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
                var mainDevice = generatedJson.ToSomfyDevice();
                var newEntities = generatedJson.ToSomfyEntities();
                var convertedDevice = EntityDeviceConverter.ConvertToDevice(mainDevice, addedName);
                foreach (var entity in newEntities)
                {
                    var convertedEntity = EntityDeviceConverter.ConvertToEntity(entity, SingletonSomfyApiService);
                    convertedDevice.SomfyEntities.Add(convertedEntity as SomfyEntity);
                }
                return convertedDevice;
            }
            catch
            {
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
                if (resultDevices == null || resultDevices.Length == 0)
                {
                    throw new Exception("No devices found");
                }

                var toAddEntites = resultDevices.ToTuyaEntities();

                var convertedDevice = EntityDeviceConverter.ConvertToDevice(toAddEntites.First(), addedName);

                foreach (var entity in toAddEntites)
                {
                    var convertedEntity = EntityDeviceConverter.ConvertToEntity(entity, SingletonTuyaApiService);
                    convertedDevice.TuyaEntities.Add(convertedEntity as TuyaEntity);
                }
                return convertedDevice;
            }
            catch
            {
                throw;
            }
        }

        internal static async Task<HttpResponseMessage> GenerateTuyaTokens(string username, string password, string region)
        {
            var SingletonTuyaApiService = TuyaApiService.GetInstance();
            SingletonTuyaApiService.SetUrl(region);

            var response = await SingletonTuyaApiService.Login(username, password, region);
            if (response == HttpStatusCode.Unauthorized)
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("Login failed") };
            }
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("Login successful") };
        }

    }
}

