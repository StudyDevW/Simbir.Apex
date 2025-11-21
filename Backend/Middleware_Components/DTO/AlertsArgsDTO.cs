using Middleware_Components.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO
{
    public class AlertsArgsDTO
    {
        public int from {  get; set; }

        public int count { get; set; }

        public AlertStatus filterStatus { get; set; }

        public SeverityStatus filterSeverity { get; set; }
    }
}
