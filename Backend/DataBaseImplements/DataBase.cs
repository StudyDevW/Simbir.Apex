using DataBaseImplement.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataBaseImplement {
    public class DataBase : DbContext {
        public DbSet<DbSimbirService> SimbirServices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseNpgsql("Host=localhost;port=5432;Database=ServiceManager;Username=postgres;Password=root")
                    .EnableDetailedErrors();
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}