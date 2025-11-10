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
                    description = "Possibly someone trying to bruteforce login credentials",
                    name = "Bruteforce detected",
                    severity = "HIGH",
                    logic = "action=LOGIN_FAIL;count=5",
                    created_at= DateTime.UtcNow,
                    status = "ACTIVE",
                    updated_at = DateTime.UtcNow,
                    created_by = Guid.Parse("34e833c4-f431-41ad-b202-adc5ceb8eecd")
                },
                new RulesTable()
                {
                    Id = Guid.NewGuid(),
                    description = "Possibly malicious process has been started",
                    name = "Process started",
                    severity = "HIGH",
                    logic = "action=PROCESS_START",
                    created_at = DateTime.UtcNow,
                    status = "ACTIVE",
                    updated_at = DateTime.UtcNow,
                    created_by = Guid.Parse("34e833c4-f431-41ad-b202-adc5ceb8eecd")
                },
                new RulesTable()
                {
                    Id = Guid.NewGuid(),
                    description = "Unusual amount of file deletions detected",
                    name = "Multiple file deletions",
                    severity = "HIGH",
                    logic = "action=FILE_DELETE;count=3",
                    created_at = DateTime.UtcNow,
                    status = "ACTIVE",
                    updated_at = DateTime.UtcNow,
                    created_by = Guid.Parse("34e833c4-f431-41ad-b202-adc5ceb8eecd")
                },
                new RulesTable()
                {
                    Id = Guid.NewGuid(),
                    description = "High frequency of new network connections detected",
                    name = "Frequent network connections",
                    severity = "HIGH",
                    logic = "action=NETWORK_CONNECT;count=8",
                    created_at = DateTime.UtcNow,
                    status = "ACTIVE",
                    updated_at = DateTime.UtcNow,
                    created_by = Guid.Parse("34e833c4-f431-41ad-b202-adc5ceb8eecd")
                },
                new RulesTable()
                {
                    Id = Guid.NewGuid(),
                    description = "Massive file modification activity detected",
                    name = "Multiple file edits",
                    severity = "HIGH",
                    logic = "action=FILE_EDIT;count=5",
                    created_at = DateTime.UtcNow,
                    status = "ACTIVE",
                    updated_at = DateTime.UtcNow,
                    created_by = Guid.Parse("34e833c4-f431-41ad-b202-adc5ceb8eecd")
                },
                new RulesTable()
                {
                    Id = Guid.NewGuid(),
                    description = "System configuration has been modified",
                    name = "Configuration changed",
                    severity = "HIGH",
                    logic = "action=CONFIG_CHANGE",
                    created_at = DateTime.UtcNow,
                    status = "ACTIVE",
                    updated_at = DateTime.UtcNow,
                    created_by = Guid.Parse("34e833c4-f431-41ad-b202-adc5ceb8eecd")
                },
                new RulesTable()
                {
                    Id = Guid.NewGuid(),
                    description = "Suspiciously high number of file openings",
                    name = "High file access activity",
                    severity = "HIGH",
                    logic = "action=FILE_OPEN;count=20",
                    created_at = DateTime.UtcNow,
                    status = "ACTIVE",
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
