using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    public class Definition
    {
        public List<State> States { get; set; }
        public string WidgetName { get; set; }
        public string Type { get; set; }
        public List<Command> Commands { get; set; }
        public string UiClass { get; set; }
    }
}
