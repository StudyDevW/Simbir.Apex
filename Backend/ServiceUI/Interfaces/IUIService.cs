using Middleware_Components.DTO;
using Middleware_Components.DTO.Enums;
using Middleware_Components.DTO.Pagination;

namespace ServiceUI.Interfaces
{
    public interface IUIService
    {
        public Task<string?> ClientSignOut(string bearer_key);

        public Task<AuthTokenInfoWR?> AuthPart(string username, string password);

        public Task<AuthTokenInfoWR?> RefreshClientSession(string refreshTokenDTO);

        public Task AddNewUser(UserAddDTO dtoObj, string email, string token);

        public Task ChangeUser(UserChangeDTO dtoObj, Guid id, string token);

        public Task DeleteUser(Guid idUser, string token);

        public Task<PaginationOut<List<UserGetDTO>>?> GetAllUsers(int from, int count, string token);

        public Task<UserGetDTO?> GetUser(Guid idUser, string token);

        public Task<List<List<TimedRuleDTO>?>> GetTimedRules(string token);

        public Task CreateRuleTimed(AddRuleDTO dtoObj, string token);

        public Task AcceptRule(string ruleName, Guid idUser, string token);

        public Task<GetRuleDTO?> GetRuleFromDB(Guid ruleId, string token);

        public Task<List<GetRuleDTO>?> GetRulesFromDB(string token);

        public Task<GetAlertDTO?> GetAlertFromDB(Guid alertId, string token);

        public Task<PaginationOut<List<GetAlertDTO>>?> GetAlertsFromDB(AlertsArgsDTO dtoObj, string token);

        public Task<GetEventDTO?> GetEventFromDB(Guid eventId, string token);

        public Task<PaginationOut<List<GetEventDTO>>?> GetEventsFromDB(int from, int count, string token);

        public Task<MeDTO> GetInfoMe(string token);
    }
}
