using Middleware_Components.DTO;

namespace ServiceUI.Interfaces
{
    public interface IUIService
    {
        public Task<string?> ClientSignOut(string bearer_key);

        public Task<AuthPairTokens?> AuthPart(string username, string password);
    }
}
