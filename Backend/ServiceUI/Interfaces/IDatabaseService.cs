using Middleware_Components.DTO;
using Middleware_Components.DTO.Enums;
using Middleware_Components.DTO.Pagination;
using Middleware_Components.JWT.DTO.CheckUsers;

namespace ServiceUI.Interfaces
{
    public interface IDatabaseService
    {
        public Task<Auth_CheckSuccess?> CheckUserAuth(string username, string password);

        public Task<RegisterMailDTO> AddUser(UserAddDTO dtoObj);

        public Task ChangeUser(UserChangeDTO dtoObj, Guid userId);

        public Task DeleteUser(Guid userId);

        public Task<PaginationOut<List<UserGetDTO>>?> GetAllUsers(int from, int count);

        public Task<UserGetDTO?> GetUser(Guid userId);

        public Task RuleFillUp(TimedRuleDTO dtoObj);

        public Task<GetRuleDTO?> GetRuleFromDB(Guid ruleId);

        public Task<List<GetRuleDTO>> GetAllRulesFromDB();

        public Task<GetAlertDTO?> GetAlertFromDB(Guid alertId);

        public Task<PaginationOut<List<GetAlertDTO>>?> GetAllAlertsFromDB(int from, int count);

        public Task<GetEventDTO?> GetEventFromDB(Guid eventId);

        public Task<PaginationOut<List<GetEventDTO>>?> GetAllEventsFromDB(int from, int count);

        public Task<List<Guid>> CollectAllIdUsers();

        public Task<MeDTO?> GetMeInfo(Guid userId);

        public Task SetOfflineStatus(Guid userId);

        public Task UpdateAlertStatus(Guid alertId, Guid userId, AlertStatus statusFill);
    }
}
