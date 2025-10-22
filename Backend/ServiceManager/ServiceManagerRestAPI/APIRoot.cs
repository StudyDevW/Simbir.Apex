using Contracts.HelperModels;

namespace ServiceManagerRestAPI {
    public class APIRoot {
        private static readonly User Root = new();

        public static void GetConfiguration(IConfiguration configuration)
        {
            Root.Login = configuration["SUPERUSER_NAME"] ?? throw new Exception("superuser name or pass not set");
            Root.Password = configuration["SUPERUSER_PASSWORD"] ?? throw new Exception("superuser name or pass not set");
        }

        public static string GetRootLogin() { return Root.Login; }
        public static string GetRootPassword() { return Root.Password; }
    }
}
