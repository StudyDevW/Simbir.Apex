using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Middleware_Components.DTO;
using Middleware_Components.DTO.Pagination;
using Middleware_Components.JWT.DTO.CheckUsers;
using ServiceUI.Interfaces;
using ServiceUI.Tables;
using Sprache;
using System.Collections.Generic;
using System.Data;

namespace ServiceUI.Services
{
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

        public async Task<Auth_CheckSuccess?> CheckUserAuth(string username, string password)
        {
            var selectedUser = await _dbcontext.usersTable.Where(c => c.username == username && c.password == password)
                .FirstOrDefaultAsync();

            if (selectedUser != null)
            {
                selectedUser.status = "online";
                selectedUser.last_login = DateTime.UtcNow;
                await _dbcontext.SaveChangesAsync();

                return new Auth_CheckSuccess() { Id = selectedUser.Id, roles = selectedUser.roles.ToList(), username = selectedUser.username };
            }

            return null;
        }

        public async Task SetOfflineStatus(Guid userId)
        {
            var selectedUser = await _dbcontext.usersTable.Where(c => c.Id == userId).FirstOrDefaultAsync();

            if (selectedUser != null )
            {
                selectedUser.status = "offline";
                await _dbcontext.SaveChangesAsync();
            }
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

        //Функа с пагинацией - Антон
        public async Task<PaginationOut<List<UserGetDTO>>?> GetAllUsers(int from, int count)
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

            var usersAllSelected = await userTableSelect.ToListAsync();

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
                    status = user.status,
                    roles = user.roles,
                    last_login = user.last_login,
                    created_at = user.created_at
                };

                retFunc.Add(userGetDto);
            }

            if (usersAllSelected.Count > 0)
                return new PaginationOut<List<UserGetDTO>>(retFunc, from, count, totalCount);

            return null;
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
                    roles = userSelected.roles,
                    created_at = userSelected.created_at,
                    last_login = userSelected.last_login,
                    status = userSelected.status
                };
            }
            else
                throw new Exception("user_not_found");

        }

 
        public async Task RuleFillUp(TimedRuleDTO dtoObj)
        {
            RulesTable rulesTable = new RulesTable()
            {
                name = dtoObj.name,
                description = dtoObj.description,
                logic = dtoObj.logic,
                severity = dtoObj.severity,
                status = dtoObj.status,
                created_at = dtoObj.created_at,
                updated_at = null,
                created_by = dtoObj.created_by
            };

            await _dbcontext.rulesTable.AddAsync(rulesTable);

            await _dbcontext.SaveChangesAsync();

        }

        public async Task<GetRuleDTO?> GetRuleFromDB(Guid ruleId)
        {
            var selectedRule = await _dbcontext.rulesTable.Where(c => c.Id == ruleId).FirstOrDefaultAsync();

            if (selectedRule != null)
                return new GetRuleDTO()
                {
                    id = ruleId,
                    name = selectedRule.name,
                    description = selectedRule.description,
                    logic = selectedRule.logic,
                    severity = selectedRule.severity,
                    status = selectedRule.status,
                    created_by = selectedRule.created_by,
                    created_at = selectedRule.created_at,
                    updated_at = selectedRule.updated_at
                };

            return null;
        }


        public async Task<List<GetRuleDTO>> GetAllRulesFromDB()
        {
            List<GetRuleDTO> rulesAll = new List<GetRuleDTO>();

            var selectedRules = await _dbcontext.rulesTable.ToListAsync();

            if (selectedRules != null)
                foreach (var rule in selectedRules)
                {
                    GetRuleDTO getRuleDTO = new GetRuleDTO()
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
                    };

                    rulesAll.Add(getRuleDTO);
                }


            return rulesAll;
        }

        public async Task<GetAlertDTO?> GetAlertFromDB(Guid alertId)
        {
            var selectedAlert = await _dbcontext.alertsTable.Where(c => c.Id == alertId).FirstOrDefaultAsync();

            if (selectedAlert != null)
                return new GetAlertDTO()
                {
                    id = alertId,
                    rule_id = selectedAlert.Id,
                    assigned_to = selectedAlert.assigned_to,
                    hostname = selectedAlert.hostname,
                    title = selectedAlert.title,
                    description = selectedAlert.description,
                    status = selectedAlert.status,
                    severity = selectedAlert.severity,
                    raw_data = selectedAlert.raw_data,  
                    created_at = selectedAlert.created_at,
                    closed_at = selectedAlert.closed_at,
                    resolution_notes = selectedAlert.resolution_notes
                };

            return null;
        }

        public async Task<PaginationOut<List<GetAlertDTO>>?> GetAllAlertsFromDB(int from, int count)
        {
            var totalCount = await _dbcontext.alertsTable.CountAsync();

            var alertsTableSelect = _dbcontext.alertsTable.AsQueryable();

            if (from > 0 && count > 0)
                alertsTableSelect = alertsTableSelect.Skip(from).Take(count);
            else if (from <= 0 && count > 0)
                alertsTableSelect = alertsTableSelect.Take(count);
            else if (from > 0 && count <= 0)
                throw new Exception("danger_output");
            else
                alertsTableSelect = alertsTableSelect.Take(_defaultMaxPageSize);

            var selectedAlerts = await alertsTableSelect.ToListAsync();

            List<GetAlertDTO> alertsAll = new List<GetAlertDTO>();

            if (selectedAlerts != null)
                foreach (var alert in selectedAlerts)
                {
                    GetAlertDTO getAlertDTO = new GetAlertDTO()
                    {
                        id = alert.Id,
                        rule_id = alert.Id,
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
                    };

                    alertsAll.Add(getAlertDTO);
                }

            if (alertsAll.Count > 0)
                return new PaginationOut<List<GetAlertDTO>>(alertsAll, from, count, totalCount);

            return null;
        }

        public async Task<GetEventDTO?> GetEventFromDB(Guid eventId)
        {
            var selectedEvent = await _dbcontext.eventsTable.Where(c => c.Id == eventId).FirstOrDefaultAsync();

            if (selectedEvent != null)
                return new GetEventDTO()
                {
                    id = eventId,
                    isLan = selectedEvent.isLan,
                    device = selectedEvent.device,
                    timestamp = selectedEvent.timestamp,
                    severity = selectedEvent.severity.ToString(),
                    category = selectedEvent.eventId
                };

            return null;
        }

        public async Task<PaginationOut<List<GetEventDTO>>?> GetAllEventsFromDB(int from, int count)
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

            var selectedEvents = await eventsTableSelect.ToListAsync();

            List<GetEventDTO> eventsAll = new List<GetEventDTO>();

            if (selectedEvents != null)
                foreach (var event_ in selectedEvents)
                {
                    GetEventDTO getAlertDTO = new GetEventDTO()
                    {
                        id = event_.Id,
                        isLan = event_.isLan,
                        device = event_.device,
                        timestamp = event_.timestamp,
                        severity = event_.severity.ToString(),
                        category = event_.eventId
                    };

                    eventsAll.Add(getAlertDTO);
                }


            if (eventsAll.Count > 0)
                return new PaginationOut<List<GetEventDTO>>(eventsAll, from, count, totalCount);

            return null;
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
            var selectedUser = await _dbcontext.usersTable.Where(c => c.Id == userId).FirstOrDefaultAsync();

            if (selectedUser != null)
            {
                MeDTO dtoUser = new MeDTO()
                {
                    id = userId,
                    first_name = selectedUser.first_name,
                    last_name = selectedUser.last_name,
                    username = selectedUser.username,
                    phone_number = selectedUser.phone_number,
                    photo_url = selectedUser.photo_url,
                    last_login = selectedUser.last_login,
                    created_at = selectedUser.created_at,
                    roles = selectedUser.roles,
                    status = selectedUser.status
                };

                return dtoUser;
            }

            return null;
        }
    }
}
