using DataBaseImplement.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataBaseImplement {
    public class DataBase : DbContext {
        public DbSet<DbSimbirService> SimbirServices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer("@");
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}