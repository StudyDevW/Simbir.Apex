using Middleware_Components.DTO.Enums;
using ServiceUI.Tables.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceUI.Tables
{
    public class EventsTable : IId
    {
        public bool isLan { get; set; }

        public string device { get; set; }

        public string eventId { get; set; }

        public SeverityStatus severity { get; set; }

        public DateTime timestamp { get; set; }
    }
}
