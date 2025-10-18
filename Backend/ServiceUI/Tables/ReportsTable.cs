using ServiceUI.Tables.Helpers;

namespace ServiceUI.Tables
{
    public class ReportsTable : IId
    {
        public Guid generated_by { get; set; }

        public string report_type { get; set; }

        public string report_data { get; set; }

        public DateTime generated_at { get; set; }
    }
}
