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
        public bool Available { get; set; }
        public int Type { get; set; }
        public string Label { get; set; }
        public bool Enabled { get; set; }
        public string ControllableName { get; set; }
        public Definition Definition { get; set; }
        public long CreationTime { get; set; }
        public long LastUpdateTime { get; set; }
        public string Widget { get; set; }
        public string UiClass { get; set; }
        public string PlaceOID { get; set; }
        public string OID { get; set; }
        public List<Attribute> Attributes { get; set; } = new();
    }
}
