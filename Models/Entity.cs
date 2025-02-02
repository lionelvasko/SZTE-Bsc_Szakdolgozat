using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    public class Entity
    {
        public string DeviceURL { get; set; }
        public int SubsystemId { get; set; }
        public bool Available { get; set; }
        public int Type { get; set; }
        public string Label { get; set; }
        public bool Synced { get; set; }
        public List<State> States { get; set; }
        public List<Attribute> Attributes { get; set; }
        public bool Enabled { get; set; }
        public string ControllableName { get; set; }
        public Definition Definition { get; set; }
        public long CreationTime { get; set; }
    }
}
