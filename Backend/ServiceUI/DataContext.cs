using Microsoft.EntityFrameworkCore;
using ServiceUI.Tables;

namespace ServiceUI
{
    public class DataContext : DbContext
    {
        private readonly string _connectionString;

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        public DbSet<AlertCommentsTable> alertCommentsTable { get; set; }

        public DbSet<AlertsTable> alertsTable { get; set; }

        public DbSet<ReportsTable> reportsTable { get; set; }

        public DbSet<RulesTable> rulesTable { get; set; }

        public DbSet<UsersTable> usersTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(_connectionString);
        }
    }
}
