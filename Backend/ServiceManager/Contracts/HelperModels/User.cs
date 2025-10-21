using DataModels.HelperModels;

namespace Contracts.HelperModels {
    public class User : IUser {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
