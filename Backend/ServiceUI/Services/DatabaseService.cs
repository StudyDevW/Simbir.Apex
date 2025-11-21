using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Middleware_Components.DTO;
using Middleware_Components.DTO.Enums;
using Middleware_Components.DTO.Pagination;
using Middleware_Components.JWT.DTO.CheckUsers;
using ServiceUI.Interfaces;
using ServiceUI.Tables;
using Sprache;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace ServiceUI.Services
{
    //Первый проект когда начал использовать проекцию - Антон(Study)
    public class DatabaseService : IDatabaseService
    {
        private readonly ILogger _logger;
        private readonly DataContext _dbcontext;
        private const int _defaultMaxPageSize = 500;

        public DatabaseService(IConfiguration conf)
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("database-service-logger");
            _dbcontext = new DataContext(conf["DATABASE_CONNECT"]);
        }

        private T ContainerExceptLog<T>(string message, string tag, [CallerMemberName] string funcName = "")
        {
            var outString = $"[{tag}] {funcName}: {message}";

            _logger.LogError(outString);

            return default;
        }


        private void ContainerExceptLog(string message, string tag, [CallerMemberName] string funcName = "")
        {
            var outString = $"[{tag}] {funcName}: {message}";

            _logger.LogError(outString);
        }

        public async Task<Auth_CheckSuccess?> CheckUserAuth(string username, string password)
        {
            try
            {
                var selectedUser = await _dbcontext.usersTable.Where(c => c.username == username && c.password == password)
                .FirstOrDefaultAsync();

                if (selectedUser != null)
                {
                    selectedUser.status = "online";
                    selectedUser.last_login = DateTime.UtcNow;
                    await _dbcontext.SaveChangesAsync();

                    return new Auth_CheckSuccess { Id = selectedUser.Id, roles = selectedUser.roles.ToList(), username = selectedUser.username };
                }

                return null;
            }
            catch (Exception ex)
            {
                return ContainerExceptLog<Auth_CheckSuccess?>(ex.Message, "EXCEPTION");
            }
        }

        public async Task SetOfflineStatus(Guid userId)
        {
            try
            {
                var selectedUser = await _dbcontext.usersTable.Where(c => c.Id == userId).FirstOrDefaultAsync();

                if (selectedUser != null)
                {
                    selectedUser.status = "offline";
                    await _dbcontext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ContainerExceptLog(ex.Message, "EXCEPTION");
            }
        }

        private string GeneratePassword(int length = 12)
        {
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()-_=+<>?";

            string allChars = uppercase + lowercase + digits + special;

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                char[] password = new char[length];

                password[0] = uppercase[randomBytes[0] % uppercase.Length];
                password[1] = lowercase[randomBytes[1] % lowercase.Length];
                password[2] = digits[randomBytes[2] % digits.Length];
                password[3] = special[randomBytes[3] % special.Length];

                for (int i = 4; i < length; i++)
                {
                    password[i] = allChars[randomBytes[i] % allChars.Length];
                }

                return new string(password.OrderBy(x => randomBytes[password.Length - 1] % password.Length).ToArray());
            }
        }

        public async Task<RegisterMailDTO> AddUser(UserAddDTO dtoObj)
        {
            try
            {
                var passwordGenerated = GeneratePassword();

                await _dbcontext.usersTable.AddAsync(new UsersTable
                {
                    first_name = dtoObj.first_name,
                    last_name = dtoObj.last_name,
                    password = passwordGenerated,
                    username = dtoObj.username,
                    roles = dtoObj.roles,
                    photo_url = null,
                    phone_number = null,
                    status = "unactive",
                    created_at = DateTime.UtcNow,
                    last_login = null
                });

                await _dbcontext.SaveChangesAsync();

                return new RegisterMailDTO
                {
                    username = dtoObj.username,
                    password = passwordGenerated
                };
            }
            catch (Exception ex)
            {
                return ContainerExceptLog<RegisterMailDTO>(ex.Message, "EXCEPTION");
            }
        }

        public async Task ChangeUser(UserChangeDTO dtoObj, Guid userId)
        {
            try
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
            catch (Exception ex)
            {
                ContainerExceptLog(ex.Message, "EXCEPTION");
            }
        }

        public async Task DeleteUser(Guid userId)
        {
            try
            {
                var selectedUser = await _dbcontext.usersTable.Where(c => c.Id == userId).FirstOrDefaultAsync();

                if (selectedUser == null)
                    throw new Exception("user_not_found");

                _dbcontext.usersTable.Remove(selectedUser);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ContainerExceptLog(ex.Message, "EXCEPTION");
            }
        }

        //Функа с пагинацией - Антон
        public async Task<PaginationOut<List<UserGetDTO>>?> GetAllUsers(int from, int count)
        {
            try
            {
                var totalCount = await _dbcontext.usersTable.CountAsync();

                var userTableSelect = _dbcontext.usersTable.AsQueryable();

                if (from > 0 && count > 0)
                    userTableSelect = userTableSelect.Skip(from).Take(count);
                else if (from <= 0 && count > 0)
                    userTableSelect = userTableSelect.Take(count);
                else if (from > 0 && count <= 0)
                    throw new Exception("danger_output");
                else
                    userTableSelect = userTableSelect.Take(_defaultMaxPageSize);

                var usersAllSelected = await userTableSelect
                    .Select(user => new UserGetDTO()
                    {
                        id = user.Id,
                        first_name = user.first_name,
                        last_name = user.last_name,
                        username = user.username,
                        phone_number = user.phone_number,
                        photo_url = user.photo_url,
                        status = user.status,
                        roles = user.roles,
                        last_login = user.last_login,
                        created_at = user.created_at
                    })
                    .ToListAsync();

                if (usersAllSelected.Count > 0)
                    return new PaginationOut<List<UserGetDTO>>(usersAllSelected, from, count, totalCount);

                return null;
            }
            catch (Exception ex)
            {
                return ContainerExceptLog<PaginationOut<List<UserGetDTO>>?>(ex.Message, "EXCEPTION");
            }
        }

        public async Task<UserGetDTO?> GetUser(Guid userId)
        {
            try
            {
                return await _dbcontext.usersTable.Where(c => c.Id == userId)
                    .Select(user => new UserGetDTO
                    {
                        id = user.Id,
                        first_name = user.first_name,
                        last_name = user.last_name,
                        username = user.username,
                        phone_number = user.phone_number,
                        photo_url = user.photo_url,
                        roles = user.roles,
                        created_at = user.created_at,
                        last_login = user.last_login,
                        status = user.status
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return ContainerExceptLog<UserGetDTO?>(ex.Message, "EXCEPTION");
            }
        }

 
        public async Task RuleFillUp(TimedRuleDTO dtoObj)
        {
            try
            {
                await _dbcontext.rulesTable.AddAsync(new RulesTable
                {
                    name = dtoObj.name,
                    description = dtoObj.description,
                    logic = dtoObj.logic,
                    severity = dtoObj.severity,
                    status = dtoObj.status,
                    created_at = dtoObj.created_at,
                    updated_at = null,
                    created_by = dtoObj.created_by
                });

                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ContainerExceptLog(ex.Message, "EXCEPTION");
            }
        }

        public async Task<GetRuleDTO?> GetRuleFromDB(Guid ruleId)
        {
            try
            {
                return await _dbcontext.rulesTable.Where(c => c.Id == ruleId)
                    .Select(rule => new GetRuleDTO
                    {
                        id = ruleId,
                        name = rule.name,
                        description = rule.description,
                        logic = rule.logic,
                        severity = rule.severity,
                        status = rule.status,
                        created_by = rule.created_by,
                        created_at = rule.created_at,
                        updated_at = rule.updated_at
                    })
                    .FirstOrDefaultAsync();
            } 
            catch (Exception ex) {
                return ContainerExceptLog<GetRuleDTO?>(ex.Message, "EXCEPTION");
            }
        }


        public async Task<List<GetRuleDTO>> GetAllRulesFromDB()
        {
            try
            {
                return await _dbcontext.rulesTable
                    .Select(rule => new GetRuleDTO
                    {
                        id = rule.Id,
                        name = rule.name,
                        description = rule.description,
                        logic = rule.logic,
                        severity = rule.severity,
                        status = rule.status,
                        created_by = rule.created_by,
                        created_at = rule.created_at,
                        updated_at = rule.updated_at
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return ContainerExceptLog<List<GetRuleDTO>>(ex.Message, "EXCEPTION");
            }
        }

        public async Task<GetAlertDTO?> GetAlertFromDB(Guid alertId)
        {
            try
            {
                return await _dbcontext.alertsTable.Where(c => c.Id == alertId)
                    .Select(alert => new GetAlertDTO
                    {
                        id = alertId,
                        rule_id = alert.rule_id,
                        assigned_to = alert.assigned_to,
                        hostname = alert.hostname,
                        title = alert.title,
                        description = alert.description,
                        status = alert.status,
                        severity = alert.severity,
                        raw_data = alert.raw_data,
                        created_at = alert.created_at,
                        closed_at = alert.closed_at,
                        resolution_notes = alert.resolution_notes
                    })
                    .FirstOrDefaultAsync();
            } 
            catch (Exception ex)
            {
                 return ContainerExceptLog<GetAlertDTO?>(ex.Message, "EXCEPTION");
            }
        }

        public async Task<PaginationOut<List<GetAlertDTO>>?> GetAllAlertsFromDB(AlertsArgsDTO dtoObj)
        {
            try
            {
                //Вывод угроз по статусу (NEW, RESOLVED, и тд.) и по важности (Severity)
                var filteredContext = _dbcontext.alertsTable.Where(c => c.status == dtoObj.filterStatus.ToString() && c.severity == dtoObj.filterSeverity.ToString());

                var totalCount = await filteredContext.CountAsync();

                //Сортировка по дате создания (от новых угроз к старым)
                var orderedQuery = filteredContext.OrderByDescending(x => x.created_at);

                var alertsTableSelect = orderedQuery.AsQueryable();

                // Выборка/Пагинация
                if (dtoObj.from > 0 && dtoObj.count > 0)
                    alertsTableSelect = alertsTableSelect.Skip(dtoObj.from).Take(dtoObj.count);
                else if (dtoObj.from <= 0 && dtoObj.count > 0)
                    alertsTableSelect = alertsTableSelect.Take(dtoObj.count);
                else if (dtoObj.from > 0 && dtoObj.count <= 0)
                    throw new Exception("danger_output");
                else
                    alertsTableSelect = alertsTableSelect.Take(_defaultMaxPageSize);

                var selectedAlerts = await alertsTableSelect
                    .Select(alert => new GetAlertDTO
                    {
                        id = alert.Id,
                        rule_id = alert.rule_id,
                        assigned_to = alert.assigned_to,
                        hostname = alert.hostname,
                        title = alert.title,
                        description = alert.description,
                        status = alert.status,
                        severity = alert.severity,
                        raw_data = alert.raw_data,
                        created_at = alert.created_at,
                        closed_at = alert.closed_at,
                        resolution_notes = alert.resolution_notes
                    })
                    .ToListAsync();

                if (selectedAlerts.Count > 0)
                    return new PaginationOut<List<GetAlertDTO>>(selectedAlerts, dtoObj.from, dtoObj.count, totalCount);

                return null;
            }
            catch (Exception ex)
            {
                return ContainerExceptLog<PaginationOut<List<GetAlertDTO>>?>(ex.Message, "EXCEPTION");
            }
        }

        public async Task<GetEventDTO?> GetEventFromDB(Guid eventId)
        {
            try
            {
                return await _dbcontext.eventsTable.Where(c => c.Id == eventId)
                    .Select(selectedEvent => new GetEventDTO
                    {
                        id = eventId,
                        isLan = selectedEvent.isLan,
                        device = selectedEvent.device,
                        timestamp = selectedEvent.timestamp,
                        severity = selectedEvent.severity.ToString(),
                        category = selectedEvent.eventId
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex) 
            {
                return ContainerExceptLog<GetEventDTO?>(ex.Message, "EXCEPTION");
            }
        }

        public async Task<PaginationOut<List<GetEventDTO>>?> GetAllEventsFromDB(int from, int count)
        {
            try
            {
                var totalCount = await _dbcontext.eventsTable.CountAsync();

                var eventsTableSelect = _dbcontext.eventsTable.AsQueryable();

                if (from > 0 && count > 0)
                    eventsTableSelect = eventsTableSelect.Skip(from).Take(count);
                else if (from <= 0 && count > 0)
                    eventsTableSelect = eventsTableSelect.Take(count);
                else if (from > 0 && count <= 0)
                    throw new Exception("danger_output");
                else
                    eventsTableSelect = eventsTableSelect.Take(_defaultMaxPageSize);

                var selectedEvents = await eventsTableSelect
                    .Select(event_ => new GetEventDTO
                    {
                        id = event_.Id,
                        isLan = event_.isLan,
                        device = event_.device,
                        timestamp = event_.timestamp,
                        severity = event_.severity.ToString(),
                        category = event_.eventId
                    })
                    .ToListAsync();

                if (selectedEvents.Count > 0)
                    return new PaginationOut<List<GetEventDTO>>(selectedEvents, from, count, totalCount);

                return null;
            }
            catch (Exception ex)
            {
                return ContainerExceptLog<PaginationOut<List<GetEventDTO>>?>(ex.Message, "EXCEPTION");
            }
        }

        public async Task<List<Guid>> CollectAllIdUsers()
        {
            List<Guid> idsOut = new List<Guid>();

            var selectedUsers = await _dbcontext.usersTable.ToListAsync();

            foreach (var user in selectedUsers)
                idsOut.Add(user.Id);

            return idsOut;
        }

        public async Task<MeDTO?> GetMeInfo(Guid userId)
        {
            try
            {
                return await _dbcontext.usersTable.Where(c => c.Id == userId)
                    .Select(user => new MeDTO
                    {
                        id = userId,
                        first_name = user.first_name,
                        last_name = user.last_name,
                        username = user.username,
                        phone_number = user.phone_number,
                        photo_url = user.photo_url,
                        last_login = user.last_login,
                        created_at = user.created_at,
                        roles = user.roles,
                        status = user.status
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return ContainerExceptLog<MeDTO?>(ex.Message, "EXCEPTION");
            }
        }

        public async Task UpdateAlertStatus(Guid alertId, Guid userId, AlertStatus statusFill, bool workEquip)
        {
            var selectedAlert = await _dbcontext.alertsTable.Where(c => c.Id == alertId).FirstOrDefaultAsync();

            if (selectedAlert != null && selectedAlert.status == AlertStatus.NEW.ToString())
            {
                if (workEquip) 
                    selectedAlert.assigned_to = userId;

                selectedAlert.status = statusFill.ToString();

                await _dbcontext.SaveChangesAsync();
            }
            else
                throw new Exception("status_update_failed");
        }
    }
}
