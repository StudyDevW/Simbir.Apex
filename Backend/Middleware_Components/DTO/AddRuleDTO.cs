using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO
{
    public class AddRuleDTO
    {
        public string name { get; set; }

        public string description { get; set; } 

        public string logic { get; set; }

        public string severity { get; set; }

        public string status { get; set; }
    }
}
