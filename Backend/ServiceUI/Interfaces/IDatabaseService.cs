using Middleware_Components.JWT.DTO.CheckUsers;

namespace ServiceUI.Interfaces
{
    public interface IDatabaseService
    {
        public Task<Auth_CheckSuccess?> CheckUserAuth(string username, string password);

    }
}
