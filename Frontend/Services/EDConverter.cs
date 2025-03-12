using SomfyAPI.Models;
namespace Szakdoga.Services
{
    internal static class EDConverter
    {
        internal static Models.Entity ConvertToEntity(object obj)
        {
            var returnEntity = new Models.Entity();
            var frontendEntities = new List<Models.Entity>();
            if (obj.GetType() == typeof(SomfyAPI.Models.Entity))
            {
                var ent = obj as SomfyAPI.Models.Entity;
                returnEntity.Id = ent.DeviceURL;
                returnEntity.Name = ent.Label;
                returnEntity.Picture = "";

            }
            else if (obj.GetType() == typeof(TuyaAPI.Models.Entity))
            {
                var ent = obj as TuyaAPI.Models.Entity;
                returnEntity.Id = ent.Id;
                returnEntity.Name = ent.Name;
                returnEntity.Picture = ent.Icon;
            }
            else
            {
                throw new ArgumentException("Invalid type");
            }
            return returnEntity;
        }

        internal static Models.Device ConvertToDevice(object obj)
        {
            var returnDevice = new Models.Device();
            if (obj.GetType() == typeof(SomfyAPI.Models.Device))
            {
                var ent = obj as SomfyAPI.Models.Device;
                returnDevice.Id = ent.Id.ToString();
                returnDevice.GatewayId = ent.GatewayId;
                returnDevice.Platform = "Somfy";
                returnDevice.Entities = new List<Models.Entity>();
            }
            else if (obj.GetType() == typeof(TuyaAPI.Models.Entity))
            {
                var ent = obj as TuyaAPI.Models.Entity;
                returnDevice.Id = ent.Id;
                returnDevice.Platform = "Tuya";
                returnDevice.Entities = new List<Models.Entity>();
            }
            else
            {
                throw new ArgumentException("Invalid type");
            }
            return returnDevice;
        }
    }
}
