using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Middleware_Components.DTO;
using Middleware_Components.JWT.DTO.CheckUsers;
using ServiceUI.Interfaces;
using ServiceUI.Tables;
using System.Collections.Generic;

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

        public async Task AddUser(UserAddDTO dtoObj)
        {
            await _dbcontext.usersTable.AddAsync(new UsersTable()
            {
                first_name = dtoObj.first_name,
                last_name = dtoObj.last_name,
                password = dtoObj.password,
                username = dtoObj.username,
                roles = dtoObj.roles,
                photo_url = dtoObj.photo_url,
                phone_number = dtoObj.phone_number,
                status = "offline",
                created_at = DateTime.UtcNow,
                last_login = null
            });

            await _dbcontext.SaveChangesAsync();
        }

        public async Task ChangeUser(UserChangeDTO dtoObj, Guid userId)
        {
            var selectedUser = await _dbcontext.usersTable.Where(c => c.Id == userId).FirstOrDefaultAsync();

            if (selectedUser == null)
                throw new Exception("user_not_found");

            if (selectedUser.username != dtoObj.username && dtoObj.username != null)
            {
                selectedUser.username = dtoObj.username;
                await _dbcontext.SaveChangesAsync();
            }

            if (selectedUser.last_name != dtoObj.last_name && dtoObj.last_name != null)
            {
                selectedUser.last_name = dtoObj.last_name;
                await _dbcontext.SaveChangesAsync();
            }

            if (selectedUser.first_name != dtoObj.first_name && dtoObj.first_name != null)
            {
                selectedUser.first_name = dtoObj.first_name;
                await _dbcontext.SaveChangesAsync();
            }

            if (selectedUser.phone_number != dtoObj.phone_number && dtoObj.phone_number != null)
            {
                selectedUser.phone_number = dtoObj.phone_number;
                await _dbcontext.SaveChangesAsync();
            }

            if (selectedUser.password != dtoObj.password && dtoObj.password != null)
            {
                selectedUser.password = dtoObj.password;
                await _dbcontext.SaveChangesAsync();
            }

            if (selectedUser.roles != dtoObj.roles && dtoObj.roles != null)
            {
                selectedUser.roles = dtoObj.roles;
                await _dbcontext.SaveChangesAsync();
            }

            if (selectedUser.photo_url != dtoObj.photo_url && dtoObj.photo_url != null)
            {
                selectedUser.photo_url = dtoObj.photo_url;
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(Guid userId)
        {
            var selectedUser = await _dbcontext.usersTable.Where(c => c.Id == userId).FirstOrDefaultAsync();

            if (selectedUser == null)
                throw new Exception("user_not_found");

            _dbcontext.usersTable.Remove(selectedUser);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<UserGetDTO>?> GetAllUsers()
        {
            var usersAllSelected = await _dbcontext.usersTable.ToListAsync();

            List<UserGetDTO> retFunc = new List<UserGetDTO>();

            foreach (var user in usersAllSelected)
            {
                UserGetDTO userGetDto = new UserGetDTO()
                {
                    id = user.Id,
                    first_name = user.first_name,
                    last_name = user.last_name,
                    username = user.username,
                    phone_number = user.phone_number,
                    photo_url = user.photo_url,
                    roles = user.roles
                };

                retFunc.Add(userGetDto);
            }

            return usersAllSelected.Count > 0 ? retFunc : null;
        }

        public async Task<UserGetDTO?> GetUser(Guid userId)
        {
            var userSelected = await _dbcontext.usersTable.Where(c => c.Id == userId).FirstOrDefaultAsync();

            if (userSelected != null)
            {
                return new UserGetDTO()
                {
                    id = userSelected.Id,
                    first_name = userSelected.first_name,
                    last_name = userSelected.last_name,
                    username = userSelected.username,
                    phone_number = userSelected.phone_number,
                    photo_url = userSelected.photo_url,
                    roles = userSelected.roles
                };
            }
            else
                throw new Exception("user_not_found");

        }
    }
}
