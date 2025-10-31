using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO
{
    public class GetRuleDTO
    {
        public Guid id { get; set; }
        public string name { get; set; }

        public string description { get; set; }

        public string logic { get; set; }

        public string severity { get; set; }

        public string status { get; set; }

        public Guid created_by { get; set; }

        public DateTime created_at { get; set; }

        public DateTime? updated_at { get; set; }
    }
}
