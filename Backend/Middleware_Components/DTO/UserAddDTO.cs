using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO
{
    public class UserAddDTO
    {
        public string first_name { get; set; }

        public string? last_name { get; set; }

        public string[] roles { get; set; }

        public string username { get; set; }
    }
}
