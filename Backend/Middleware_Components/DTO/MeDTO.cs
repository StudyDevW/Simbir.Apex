using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO
{
    public class MeDTO
    {
        public Guid id { get; set; }
        public string first_name { get; set; }

        public string? last_name { get; set; }

        public string? phone_number { get; set; }

        public string? photo_url { get; set; }

        public string[] roles { get; set; }

        public string username { get; set; }

        public string status { get; set; }

        public DateTime created_at { get; set; }

        public DateTime? last_login { get; set; }
    }
}
