using Szakdoga.Models;
using Device = Szakdoga.Models.Device;

namespace Szakdoga.Services
{
    internal static class EntityDeviceConverter
    {
        internal static Entity ConvertToEntity(object obj)
        {
            var frontendEntities = new List<Entity>();
            if (obj.GetType() == typeof(SomfyAPI.Models.Entity))
            {
                var returnEntity = new SomfyEntity();
                var ent = obj as SomfyAPI.Models.Entity;
                returnEntity.Id = ent.DeviceURL;
                returnEntity.Name = ent.Label;
                returnEntity.Platform = "Somfy";
                return returnEntity;
            }
            else if (obj.GetType() == typeof(TuyaAPI.Models.Device))
            {
                var returnEntity = new TuyaEntity();
                var ent = obj as TuyaAPI.Models.Device;
                returnEntity.Id = ent.Id;
                returnEntity.Name = ent.Name;
                returnEntity.Icon = ent.Icon;
                returnEntity.Platform = "Tuya";
                returnEntity.State = ent.Data.State;
                returnEntity.Brightness = ent.Data.Brightness;
                returnEntity.ColorMode = ent.Data.ColorMode;
                returnEntity.ColorTemp = ent.Data.ColorTemp;
                returnEntity.Online = ent.Data.Online;

                return returnEntity;
            }
            else
            {
                throw new ArgumentException("Invalid type");
            }
        }

        internal static Device ConvertToDevice(object obj)
        {
            if (obj.GetType() == typeof(SomfyAPI.Models.Device))
            {
                var returnDevice = new Models.SomfyDevice();
                var ent = obj as SomfyAPI.Models.Device;
                returnDevice.Id = ent.Id.ToString();
                returnDevice.GatewayId = ent.GatewayId;
                returnDevice.Platform = "Somfy";
                returnDevice.Entities = new List<Models.Entity>();
                returnDevice.Creation_Time = CreateTime();
                return returnDevice;
            }
            else if (obj.GetType() == typeof(TuyaAPI.Models.Device))
            {
                var returnDevice = new Models.TuyaDevice();
                var ent = obj as TuyaAPI.Models.Device;
                returnDevice.Platform = "Tuya";
                returnDevice.Entities = new List<Models.Entity>();
                returnDevice.Creation_Time = CreateTime();
                return returnDevice;
            }
            else
            {
                throw new ArgumentException("Invalid type");
            }
        }

        private static string CreateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
