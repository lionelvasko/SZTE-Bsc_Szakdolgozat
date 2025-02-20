using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomfyAPI.Models
{
    public class Definition
    {
        public int Id { get; set; }
        public List<Command> Commands { get; set; } = new();
        public List<State> States { get; set; } = new();
        public List<string> UiProfiles { get; set; } = new();
        public string WidgetName { get; set; }
        public string UiClass { get; set; }
        public string QualifiedName { get; set; }
        public string Type { get; set; }
    }
}
