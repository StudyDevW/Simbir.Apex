using ServiceUI.Tables.Helpers;

namespace ServiceUI.Tables
{
    public class AlertCommentsTable : IId
    {
        public Guid alert_id { get; set; }

        public Guid user_id { get; set; }

        public string comment { get; set; } 

        public DateTime created_at { get; set; }
    }
}
