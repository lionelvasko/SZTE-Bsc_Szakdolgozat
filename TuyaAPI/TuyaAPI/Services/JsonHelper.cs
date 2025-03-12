using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TuyaAPI.Models;

namespace TuyaAPI.Services
{
    public static class JsonHelper
    {
        public static List<Entity> GetEntitiesFromJson(string json)
        {
            var setupData = JsonSerializer.Deserialize<TuyaResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var entities = new List<Entity>();

            if (setupData?.Payload != null)
            {
                foreach (var listEntity in setupData.Payload.Devices)
                {
                    entities.Add(new Entity
                    {
                        Name = listEntity.Name,
                        Icon = listEntity.Icon,
                        Id = listEntity.Id,
                        DevType = listEntity.DevType,
                        HaType = listEntity.HaType
                    });
                }
            }

            return entities;
        }
    }
}
