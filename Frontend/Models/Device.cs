using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    internal class Device
    {
        public string Name { get; set; } = string.Empty;
        public string CreationTime { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public ICollection<SomfyEntity> SomfyEntities { get; set; } = new List<SomfyEntity>();
        public ICollection<TuyaEntity> TuyaEntities { get; set; } = new List<TuyaEntity>();
    }
}
