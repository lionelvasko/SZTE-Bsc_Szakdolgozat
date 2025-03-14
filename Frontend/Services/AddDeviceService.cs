using Microsoft.JSInterop;
using SomfyAPI.Services;
using Szakdoga.Models;
using TuyaAPI.Services;

namespace Szakdoga.Services
{
    internal static class AddDeviceService
    {
        internal static async Task<Models.Device> AddSomfyDeviceAsync(string email, string password, string homeUrl)
        {
            try
            {
                var SingletonSomfyApiService = SomfyApiService.GetInstance();

                // Attempt to login with the provided credentials
                bool validLogin = await SingletonSomfyApiService.LoginAsync(email, password, homeUrl);
                if (!validLogin)
                {
                    throw new Exception("Login failed");
                }

                // Set the home URL
                SingletonSomfyApiService.SetUrl(true);

                // Attempt to get the setup JSON
                var generatedJson = await SingletonSomfyApiService.GetSetupJsonAsync();

                // Convert the JSON to device and entities
                var mainDevice = SomfyAPI.Services.JsonHelper.GetDeviceFromJson(generatedJson);
                var newEntities = SomfyAPI.Services.JsonHelper.GetEntitiesFromJson(generatedJson);

                // Convert the device and its entities
                var convertedDevice = EDConverter.ConvertToDevice(mainDevice);
                convertedDevice.Email = email;

                foreach (var entity in newEntities)
                {
                    var convertedEntity = (Szakdoga.Models.Entity)EDConverter.ConvertToEntity(entity);
                    convertedDevice.Entities.Add(convertedEntity);
                }

                return convertedDevice;
            }
            catch (Exception ex)
            {
                // Handle the error, log it, and rethrow or return null/alternative value
                Console.Error.WriteLine($"Error in AddSomfyDeviceAsync: {ex.Message}");
                // Optionally, you could return a default value or rethrow the exception
                throw; // rethrow the exception or return null
            }
        }

        internal static async Task<Models.Device> AddTuyaDeviceAsync(string username, string password, string region)
        {
            try
            {
                var SingletonTuyaApiService = TuyaApiService.GetInstance();
                SingletonTuyaApiService.SetUrl(region);

                // Attempt to login
                var response = await SingletonTuyaApiService.Login(username, password, region);
                if (response == null)
                {
                    throw new Exception("Login failed");
                }

                // Attempt to get devices
                var resultDevices = await SingletonTuyaApiService.GetDevices();
                if (resultDevices == null || !resultDevices.Any())
                {
                    throw new Exception("No devices found");
                }

                // Convert the devices
                var toAddEntites = TuyaAPI.Services.JsonHelper.GetEntitiesFromJson(resultDevices);
                var convertedDevice = EDConverter.ConvertToDevice(toAddEntites.First());
                convertedDevice.Email = username;

                // Add entities to the device
                foreach (var entity in toAddEntites)
                {
                    var convertedEntity = (Szakdoga.Models.Entity)EDConverter.ConvertToEntity(entity);
                    convertedDevice.Entities.Add(convertedEntity);
                }

                return convertedDevice;
            }
            catch (Exception ex)
            {
                // Handle the error, log it, and rethrow or return null/alternative value
                Console.Error.WriteLine($"Error in AddTuyaDeviceAsync: {ex.Message}");
                // Optionally, you could return a default value or rethrow the exception
                throw; // rethrow the exception or return null
            }
        }

    }
}

