using Microsoft.EntityFrameworkCore;
using ServiceUI.Interfaces;

namespace ServiceUI.Services
{
    public class AutoMigrationService : IAutoMigrationService
    {
        private readonly DataContext _dbcontext;

        public AutoMigrationService(IConfiguration conf)
        {
            _dbcontext = new DataContext(conf["DATABASE_CONNECT"]);
        }

        public async Task EnsureDatabaseInitializedAsync()
        {
            //Вытянул из старого проекта, все вырезал (доработаю)

          //  await _dbcontext.Database.MigrateAsync();
        }
    }
}
