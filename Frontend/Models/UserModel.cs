using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Models
{
    internal class UserModel
    {
        internal string Email { get; set; } = string.Empty;
        internal string Name { get; set; } = string.Empty;

    }
}
