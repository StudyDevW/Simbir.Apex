using Microsoft.EntityFrameworkCore;
using Npgsql;
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
            //Вытянул из старого проекта, все вырезал (доработаю)

            List<string> tableNamesArray = new List<string>()
            {
                "usersTable",
                "rulesTable",
                "reportsTable",
                "alertsTable",
                "alertCommentsTable"
            };

            List<string> tablesToBackup = new List<string>()
            {
                "usersTable",
                "rulesTable",
                "reportsTable",
                "alertsTable",
                "alertCommentsTable"
            };

            List<string> tablesNotExist = new List<string>();

            for (int i = 0; i < tableNamesArray.Count; i++)
            {
                var tableExists = await CheckIfTableExistsAsync(_dbcontext, tableNamesArray[i]);

                if (!tableExists)
                {
                    //_logger.LogWarning($"Таблица {tableNamesArray[i]} отсутствует");

                    tablesNotExist.Add(tableNamesArray[i]);
                    tablesToBackup.Remove(tableNamesArray[i]);
                }
            }

            //Если хоть одна таблица отсутствует
            if (tablesNotExist.Count > 0)
            {
                //Выполняем бекап всех данных из таблиц
                //if (tablesToBackup.Count > 0)
                //{
                //    _logger.LogInformation($"Есть таблицы для резервного копирования");

                //    for (int i = 0; i < tablesToBackup.Count; i++)
                //        BackupDatabase(tablesToBackup[i]);

                //    _logger.LogInformation($"Все таблицы скопированы");
                //}


                await _dbcontext.Database.EnsureDeletedAsync();
                await _dbcontext.Database.MigrateAsync();

                //_logger.LogInformation($"Миграции выполнены успешно");

                //CompleteBackup();

                //_logger.LogInformation($"Если было резервное копирование, то данные восстановлены");

                tablesNotExist.Clear();
            }
        }
    }
}
