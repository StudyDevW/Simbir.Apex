using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Middleware_Components.DTO.Enums;
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

        public DbSet<EventsTable> eventsTable { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersTable>().HasData(new UsersTable() {
                Id = Guid.Parse("34e833c4-f431-41ad-b202-adc5ceb8eecd"),
                first_name = "test",
                last_name = "user",
                phone_number = null,
                photo_url = null,
                roles = new string[] { "MANAGER" },
                username = "testuser",
                password = "root",
                status = "unknown",
                created_at = DateTime.UtcNow,
                last_login = DateTime.UtcNow
            });

            modelBuilder.Entity<RulesTable>().HasData(
                new RulesTable()
                {
                    Id = Guid.NewGuid(),
                    description = "testruledesc",
                    name = "testrulename",
                    severity = "HIGH",
                    logic = "\"logic\": \"action=LOGIN_FAIL;count=5\"",
                    created_at= DateTime.UtcNow,
                    status = "APPROVED",
                    updated_at = DateTime.UtcNow,
                    created_by = Guid.Parse("34e833c4-f431-41ad-b202-adc5ceb8eecd")
                },
                new RulesTable()
                {
                    Id = Guid.NewGuid(),
                    description = "testruledesc_second",
                    name = "testrulename_second",
                    severity = "HIGH",
                    logic = "\"logic\": \"action=LOGIN_SUCCESS;sequence=FILE_DELETE;count=1\"",
                    created_at = DateTime.UtcNow,
                    status = "APPROVED",
                    updated_at = DateTime.UtcNow,
                    created_by = Guid.Parse("34e833c4-f431-41ad-b202-adc5ceb8eecd")
                }
            );

            modelBuilder.Entity<EventsTable>()
                .Property(e => e.severity)
                .HasConversion(
                v => v.ToString().ToLower(), 
                v => (SeverityStatus)Enum.Parse(typeof(SeverityStatus), v, true)
            )
            .HasColumnType("varchar(10)");

            modelBuilder.Entity<AlertsTable>()
               .Property(e => e.severity)
               .HasConversion(
               v => v.ToString().ToLower(),
               v => (SeverityStatus)Enum.Parse(typeof(SeverityStatus), v, true) 
           )
           .HasColumnType("varchar(10)");
        }
    }
}
