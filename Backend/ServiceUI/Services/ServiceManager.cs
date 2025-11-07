using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceUI.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ServiceUI.Services {
    public class ServiceManager : IServiceManager {
        private readonly string _serviceUIAddress;
        private readonly JsonSerializerSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        public ServiceManager(IConfiguration conf)
        {
            
            _httpClient = new();
            _httpClient.BaseAddress = new Uri("http://simbirapex.backend.servicemanager:80");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new IPAddressConverter());
            _settings.Converters.Add(new IPEndPointConverter());
            _settings.Formatting = Formatting.Indented;

            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("servmanager_log");
        }

        [HttpPost]
        public Task ServiceInit()
        {
            var serviceRecord = new SimbirService {
                ServiceName = "ServiceUI",
                EndPointService = IPEndPoint.Parse("127.0.0.1:8080"),
                Id = 0
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "http://simbirapex.backend.servicemanager:80/api/main/InsertService");
            string json = JsonConvert.SerializeObject(serviceRecord, _settings);
            string postjson = JsonConvert.SerializeObject(json);
            _logger.LogInformation(postjson);

            request.Content = new StringContent(postjson.ToString(), Encoding.UTF8, "application/json");

            var response = _httpClient.SendAsync(request);
            var result = response.Result.Content.ReadAsStringAsync().Result;

            if (response.Result.IsSuccessStatusCode) {
                _logger.LogInformation("Успешно отправлено");
                return Task.FromResult(result);
            }
            else {
                throw new Exception(result);
            }            
        }
    }

    public class SimbirService
    {
        public string ServiceName { get; set; } = string.Empty;
        public IPEndPoint? EndPointService { get; set; }
        public int Id { get; set; } = 0;
    }

    public class IPAddressConverter : JsonConverter {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IPAddress);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return IPAddress.Parse((string)reader.Value);
        }
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null) { return; }
            writer.WriteValue(value.ToString());
        }
    }

    public class IPEndPointConverter : JsonConverter {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IPEndPoint);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            IPAddress address = jo["Address"].ToObject<IPAddress>(serializer);
            int port = (int)jo["Port"];
            return new IPEndPoint(address, port);
        }
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null) { return; }
            IPEndPoint ep = (IPEndPoint)value;
            JObject jo = new JObject {
                { "Address", JToken.FromObject(ep.Address, serializer) },
                { "Port", ep.Port }
            };
            jo.WriteTo(writer);
        }
    }
}
