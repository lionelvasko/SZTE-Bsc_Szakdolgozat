using Szakdoga.Models;
using Szakdoga.Requests;
using Device = Szakdoga.Models.Device;

namespace Szakdoga.Services
{
    internal static class EntityDeviceConverter
    {
        internal static Entity ConvertToEntity(object obj, object ApiCaller)
        {
            var frontendEntities = new List<Entity>();
            if (obj.GetType() == typeof(SomfyAPI.Models.Entity))
            {
                var service = ApiCaller as SomfyAPI.Services.SomfyApiService;

                var returnEntity = new SomfyEntity();
                var ent = obj as SomfyAPI.Models.Entity;
                returnEntity.Url = ent.DeviceURL;
                returnEntity.Name = ent.Label;
                returnEntity.Icon = "Resources/Images/somfy_logo.svg";
                returnEntity.Platform = "Somfy";
                returnEntity.BaseUrl = service.GetURL();
                returnEntity.CloudUsername = service.Username;
                returnEntity.CloudPasswordHashed = service.Password;
                return returnEntity;
            }
            else if (obj.GetType() == typeof(TuyaAPI.Models.Device))
            {
                var service = ApiCaller as TuyaAPI.Services.TuyaApiService;

                var returnEntity = new TuyaEntity();
                var ent = obj as TuyaAPI.Models.Device;
                returnEntity.Url = ent.Id;
                returnEntity.Name = ent.Name;
                returnEntity.Icon = ent.Icon;
                returnEntity.Platform = "Tuya";
                returnEntity.AccessToken = service.GetAccessToken();
                returnEntity.RefreshToken = service.GetRefreshToken();
                returnEntity.Region = service.GetRegion();

                return returnEntity;
            }
            else
            {
                throw new ArgumentException("Invalid type in entity convert: wrong type");
            }
        }

        internal static Device ConvertToDevice(object obj, string Name, object ApiCaller)
        {
            if (obj.GetType() == typeof(SomfyAPI.Models.Device) || obj.GetType() == typeof(TuyaAPI.Models.Device))
            {
                var returnDevice = new Models.Device();
                var ent = obj as SomfyAPI.Models.Device;

                returnDevice.Name = Name;
                returnDevice.CreationTime = CreateTime();
                returnDevice.Platform = obj is SomfyAPI.Models.Device ? "Somfy" : "Tuya";
                return returnDevice;
            }
            else
            {
                throw new ArgumentException("Invalid type in device convert: wrong type");
            }
        }

        private static string CreateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        internal static AddDeviceRequest ConvertToAddDeviceRequest(Device device)
        {
            return new AddDeviceRequest
            {
                Name = device.Name,
                Platform = device.Platform,
            };
        }

        internal static AddEntityRequest ConvertToAddEntityRequest(Entity entity)
        {
            if(entity.Platform == "Somfy")
            {
                return new AddEntityRequest
                {
                    Name = entity.Name,
                    Platform = entity.Platform,
                    Icon = entity.Icon,
                    URL = entity.Url,
                    BaseUrl = (entity as SomfyEntity)?.BaseUrl,
                    CloudUsername = (entity as SomfyEntity)?.CloudUsername,
                    CloudPasswordHashed = (entity as SomfyEntity)?.CloudPasswordHashed,
                    AccessToken = (entity as TuyaEntity)?.AccessToken,
                    RefreshToken = (entity as TuyaEntity)?.RefreshToken,
                    Region = (entity as TuyaEntity)?.Region
                };
            }
            else if(entity.Platform == "Tuya")
            {
                return new AddEntityRequest
                {
                    Name = entity.Name,
                    Platform = entity.Platform,
                    Icon = entity.Icon,
                    URL = entity.Url,
                    AccessToken = (entity as TuyaEntity)?.AccessToken,
                    RefreshToken = (entity as TuyaEntity)?.RefreshToken,
                    Region = (entity as TuyaEntity)?.Region
                };
            }
            else
            {
                throw new ArgumentException("Invalid type in entity convert: wrong type");
            }
        }

    }
}
