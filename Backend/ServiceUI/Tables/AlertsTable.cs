using Middleware_Components.DTO.Enums;
using ServiceUI.Tables.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceUI.Tables
{
    public class AlertsTable : IId
    {
        public Guid? rule_id { get; set; }

        public Guid? assigned_to { get; set; }

        public string hostname { get; set; }

        public string title { get; set; }

        public string description { get; set; } 

        public string status { get; set; }

        public string severity { get; set; }

        public string raw_data { get; set; }

        public DateTime created_at { get; set; }

        public DateTime? closed_at { get; set; }

        public string resolution_notes { get; set; }
    }
}
