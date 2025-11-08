using Middleware_Components.DTO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO
{
    public class GetEventDTO
    {
        public Guid id { get; set; }

        public bool isLan { get; set; }

        public string device { get; set; }

        public string severity { get; set; }

        public string category { get; set; }

        public DateTime timestamp { get; set; }
    }
}
