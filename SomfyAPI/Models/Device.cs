using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomfyAPI.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string GatewayId { get; set; }
        public int Type { get; set; }
        public int SubType { get; set; }
        public bool AutoUpdateEnabled { get; set; }
        public bool Alive { get; set; }
        public bool TimeReliable { get; set; }
        public Connectivity Connectivity { get; set; }
        public bool UpToDate { get; set; }
        public string UpdateStatus { get; set; }
        public bool SyncInProgress { get; set; }
        public string UpdateCriticityLevel { get; set; }
        public bool AutomaticUpdate { get; set; }
        public string Mode { get; set; }
        public string Functions { get; set; }
    }
}
