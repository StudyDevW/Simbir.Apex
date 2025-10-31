using Middleware_Components.DTO;
using Middleware_Components.JWT.DTO.CheckUsers;

namespace ServiceUI.Interfaces
{
    public interface IDatabaseService
    {
        public Task<Auth_CheckSuccess?> CheckUserAuth(string username, string password);

        public Task AddUser(UserAddDTO dtoObj);

        public Task ChangeUser(UserChangeDTO dtoObj, Guid userId);

        public Task DeleteUser(Guid userId);

        public Task<List<UserGetDTO>?> GetAllUsers();

        public Task<UserGetDTO?> GetUser(Guid userId);

        public Task RuleFillUp(TimedRuleDTO dtoObj);

        public Task<GetRuleDTO?> GetRuleFromDB(Guid ruleId);

        public Task<List<GetRuleDTO>> GetAllRulesFromDB();

        public Task<GetAlertDTO?> GetAlertFromDB(Guid alertId);

        public Task<List<GetAlertDTO>> GetAllAlertsFromDB();
    }
}
