using Middleware_Components.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO
{
    public class GetAlertDTO
    {
        public Guid id { get; set; }

        public Guid? rule_id { get; set; }

        public Guid? assigned_to { get; set; }

        public string hostname { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string status { get; set; }

        public SeverityStatus severity { get; set; }

        public string raw_data { get; set; }

        public DateTime created_at { get; set; }

        public DateTime? closed_at { get; set; }

        public string resolution_notes { get; set; }
    }
}
