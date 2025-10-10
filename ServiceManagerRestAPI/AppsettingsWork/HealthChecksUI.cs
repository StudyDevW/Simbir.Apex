using Contracts.BindingModels;
using Newtonsoft.Json;

namespace ServiceManagerRestAPI.AppsettingsWork {
   public class Config
    {
        public HealthChecksUI HealthChecksUI { get; set; }
    }
    public class HealthChecksUI {
        public List<HealthChecks> HealthChecks { get; set; } = new();
        public int EvaluationTimeInSeconds { get; set; }
    }
    public class HealthChecks
    {
        public string Name { get; set; }
        public string Uri { get; set; }
    }

    public class AppsettingsWorkService
    {
        private readonly JsonSerializerSettings jsonSettings;
        public AppsettingsWorkService() {
            jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new IPEndPointConverter());
            jsonSettings.Converters.Add(new IPAddressConverter());
            jsonSettings.Formatting = Formatting.Indented;
        }
        public void AddUpdateAppsettings(in SimbirServiceBindingModel serviceModel)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .Get<Config>();

            if (config == null) { throw new Exception("config file not found"); }

            HealthChecks checkService = new();
            checkService.Name = serviceModel.ServiceName;

            string address = serviceModel.EndPointService.Address.ToString();
            string port = serviceModel.EndPointService.Port.ToString();

            checkService.Uri = $"https://{address}:{port}";

            for (int record = 0, listLen = config.HealthChecksUI.HealthChecks.Count(); record < listLen; ++record) {
                if (config.HealthChecksUI.HealthChecks[record].Name == checkService.Name) {
                    config.HealthChecksUI.HealthChecks[record].Uri = checkService.Uri;
                    goto exit;
                }
            }
            config.HealthChecksUI.HealthChecks.Add(checkService);

            exit:
            var newJson =  JsonConvert.SerializeObject(config, jsonSettings);
            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            System.IO.File.WriteAllText(appSettingsPath, newJson);
        }
    }
}
