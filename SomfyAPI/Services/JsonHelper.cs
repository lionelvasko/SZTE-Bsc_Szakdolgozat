using SomfyAPI.Models;
using System.Text.Json;

namespace SomfyAPI.Services
{
    public static class JsonExtensions
    {
        // Cache the JsonSerializerOptions instance to avoid creating a new one for every operation
        private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static List<Entity> ToSomfyEntities(this string json)
        {
            var setupData = JsonSerializer.Deserialize<SetupResponse>(json, CachedJsonSerializerOptions);

            var entities = new List<Entity>();

            if (setupData?.Entities != null)
            {
                foreach (var listEntity in setupData.Entities)
                {
                    if (Enum.TryParse<Resources.TahomaEntityEnum>(listEntity.Label, true, out _))
                    {
                        entities.Add(new Entity
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

        public static Device? ToSomfyDevice(this string json)
        {
            var setupData = JsonSerializer.Deserialize<SetupResponse>(json, CachedJsonSerializerOptions);
            return setupData?.Devices?.FirstOrDefault();
        }
    }
}
