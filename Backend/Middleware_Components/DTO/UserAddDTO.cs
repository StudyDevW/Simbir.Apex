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

        public string? phone_number { get; set; }

        public string? photo_url { get; set; }

        public string[] roles { get; set; }

        public string username { get; set; }

        public string password { get; set; }
    }
}
