using DataBaseImplement;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ServiceManagerRestAPI {
    public class AutoMigrationService : IAutoMigrationService {
        private readonly DataBase _dbcontext;

        public AutoMigrationService(IConfiguration conf)
        {
            _dbcontext = new DataBase(conf["DATABASE_CONNECT_M"]);
        }

        private async Task<bool> CheckIfTableExistsAsync(DbContext context, string tableName)
        {
            var connection = (NpgsqlConnection)context.Database.GetDbConnection();
            await connection.OpenAsync();

            var exists = false;

            var command = new NpgsqlCommand(
                $"SELECT EXISTS (SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename = '{tableName}');",
                connection);

            exists = (bool)await command.ExecuteScalarAsync();

            await connection.CloseAsync();
            return exists;
        }

        public async Task EnsureDatabaseInitializedAsync()
        {
            List<string> tableNamesArray = new List<string>()
            {
                "SimbirServices"
            };

            List<string> tablesToBackup = new List<string>()
            {
                "SimbirServices"
            };

            List<string> tablesNotExist = new List<string>();

            for (int i = 0; i < tableNamesArray.Count; i++) {
                var tableExists = await CheckIfTableExistsAsync(_dbcontext, tableNamesArray[i]);

                if (!tableExists) {
                    tablesNotExist.Add(tableNamesArray[i]);
                    tablesToBackup.Remove(tableNamesArray[i]);
                }
            }

            if (tablesNotExist.Count > 0) {
                await _dbcontext.Database.EnsureDeletedAsync();
                await _dbcontext.Database.MigrateAsync();

                tablesNotExist.Clear();
            }
        }
    }
}
