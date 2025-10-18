using Microsoft.AspNetCore.Identity;
using ServiceUI.Interfaces;

namespace ServiceUI.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ILogger _logger;
        private readonly DataContext _dbcontext;
        //private readonly PasswordHasher<PasswordAppUser> _passwordHasher;

        public DatabaseService(IConfiguration conf)
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("database-service-logger");
       //     _passwordHasher = new PasswordHasher<PasswordAppUser>();
            _dbcontext = new DataContext(conf["DATABASE_CONNECT"]);
        }
    }
}
