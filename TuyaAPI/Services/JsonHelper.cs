using System.Text.Json;
using TuyaAPI.Models;

namespace TuyaAPI.Services
{
    public static class JsonHelper
    {
        public static List<Device> GetEntitiesFromJson(string json)
        {
            var setupData = JsonSerializer.Deserialize<TuyaResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var entities = new List<Device>();

            if (setupData?.Payload != null)
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
