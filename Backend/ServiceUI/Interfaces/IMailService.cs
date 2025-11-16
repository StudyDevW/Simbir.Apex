using Middleware_Components.DTO;

namespace ServiceUI.Interfaces
{
    public interface IMailService
    {
        public Task SendRegisterData(string email, RegisterMailDTO registerData);
    }
}
