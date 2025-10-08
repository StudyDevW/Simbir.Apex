using Contracts.HelperModels;

namespace ServiceManagerRestAPI {
    public class APIRoot {
        private static readonly User Root = new();

        public static void GetConfiguration(IConfiguration configuration)
        {
            Root.Login = configuration["Root:Login"] ?? "Admin";
            Root.Password = configuration["Root:Password"] ?? "root";
        }

        public static string GetRootLogin() { return Root.Login; }
        public static string GetRootPassword() { return Root.Password; }
    }
}
