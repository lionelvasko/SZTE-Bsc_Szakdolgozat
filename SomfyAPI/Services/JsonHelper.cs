using System.Text.Json;

namespace SomfyAPI.Services
{
    public static class JsonHelper
    {
        public static List<Models.Entity> GetEntitiesFromJson(string json)
        {
            var setupData = JsonSerializer.Deserialize<Models.SetupResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var entities = new List<Models.Entity>();

            if (setupData?.Entities != null)
            {
                foreach (var listEntity in setupData.Entities)
                {
                    if (Enum.TryParse<Resources.TahomaEntityList>(listEntity.Label, true, out _))
                    {
                        entities.Add(new Models.Entity
                        {
                            DeviceURL = listEntity.DeviceURL,
                            Available = listEntity.Available,
                            Type = listEntity.Type,
                            Label = listEntity.Label,
                            Enabled = listEntity.Enabled,
                            ControllableName = listEntity.ControllableName,
                            Definition = listEntity.Definition,
                            CreationTime = listEntity.CreationTime,
                            LastUpdateTime = listEntity.LastUpdateTime,
                            Widget = listEntity.Widget,
                            UiClass = listEntity.UiClass,
                            PlaceOID = listEntity.PlaceOID,
                            OID = listEntity.OID,
                            Attributes = listEntity.Attributes

    });
                    }

                }
            }

            return entities;
        }
        public static Models.Device? GetDeviceFromJson(string json)
        {
            var setupData = JsonSerializer.Deserialize<Models.SetupResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return setupData?.Devices?.FirstOrDefault();
        }
    }
}
