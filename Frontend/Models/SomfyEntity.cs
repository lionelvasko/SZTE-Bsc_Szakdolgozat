using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    internal class SomfyEntity : Entity
    {
        public string BaseUrl { get; set; }
        public string GatewayPin { get; set; }
        public string SessionId { get; set; }
        public string Token { get; set; }
    }
}
