using Middleware_Components.DTO;

namespace ServiceUI.Interfaces
{
    public interface IUIService
    {
        public Task<string?> ClientSignOut(string bearer_key);

        public Task<AuthPairTokens?> AuthPart(string username, string password);

        public Task<AuthPairTokens?> RefreshClientSession(string refreshTokenDTO);

        public Task AddNewUser(UserAddDTO dtoObj, string token);

        public Task ChangeUser(UserChangeDTO dtoObj, string token);

        public Task DeleteUser(Guid idUser, string token);

        public Task<List<UserGetDTO>?> GetAllUsers(string token);

        public Task<UserGetDTO?> GetUser(Guid idUser, string token);
    }
}
