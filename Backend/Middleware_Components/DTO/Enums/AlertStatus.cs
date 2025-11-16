using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO.Enums
{
    public enum AlertStatus
    {
        NEW,
        FALSE_POSITIVE,
        CONFIRMED_PRESET,
        ESCALATED,
        RESOLVED
    }
}
