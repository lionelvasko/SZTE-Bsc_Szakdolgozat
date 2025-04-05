using TuyaAPI.DTOs;
using System.Text.Json;
using TuyaAPI.Models;

namespace TuyaAPI.Services
{
    public static class JsonHelper
    {
        public static List<EntityDTO> GetEntitiesFromJson(string json)
        {
            var setupData = JsonSerializer.Deserialize<TuyaResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var entities = new List<EntityDTO>();

            if (setupData?.Payload != null)
            {
                foreach (var listEntity in setupData.Payload.Devices)
                {
                    entities.Add(new EntityDTO
                    {
                        Platform = "tuya",
                        Name = listEntity.Name,
                        Icon = listEntity.Icon,
                        URL = listEntity.Id,
                    });
                }
            }

            return entities;
        }
    }
}
