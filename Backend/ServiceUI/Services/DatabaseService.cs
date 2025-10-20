using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Middleware_Components.JWT.DTO.CheckUsers;
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

        public async Task<Auth_CheckSuccess?> CheckUserAuth(string username, string password)
        {
            var selectedUser = await _dbcontext.usersTable.Where(c => c.username == username && c.password == password)
                .FirstOrDefaultAsync();

            if (selectedUser != null) return new Auth_CheckSuccess() { Id = selectedUser.Id, roles = selectedUser.roles.ToList(), username = selectedUser.username };

            return null;
        }

    }
}
