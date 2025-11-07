using DataBaseImplement.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataBaseImplement {
    public class DataBase : DbContext {
        private readonly string _connectString;
        public DataBase(string connectString)
        {
            _connectString = connectString;
            Database.EnsureCreated();
        }
        public DataBase(DbContextOptions<DataBase> options) : base(options) {}
        public DbSet<DbSimbirService> SimbirServices { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) { optionsBuilder.UseNpgsql(_connectString); }
            base.OnConfiguring(optionsBuilder);
        }
    }
}