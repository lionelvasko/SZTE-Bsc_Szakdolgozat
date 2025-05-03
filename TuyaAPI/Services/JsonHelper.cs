using System.Text.Json;
using TuyaAPI.Models;

namespace TuyaAPI.Services
{
    public static class JsonHelperExtensions
    {
        // Cache the JsonSerializerOptions instance to avoid creating a new one for every operation
        private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static List<Device> ToTuyaEntities(this string json)
        {
            var setupData = JsonSerializer.Deserialize<TuyaResponse>(json, CachedJsonSerializerOptions);

            var entities = new List<Device>();

            if (setupData?.Payload?.Devices != null)
            {
                foreach (var listEntity in setupData.Payload.Devices)
                {
                    entities.Add(new Device
                    {
                        Name = listEntity.Name,
                        Data = listEntity.Data,
                        DevType = listEntity.DevType,
                        Icon = listEntity.Icon,
                        Id = listEntity.Id,
                        HaType = listEntity.HaType
                    });
                }
            }

            return entities;
        }
    }
}
