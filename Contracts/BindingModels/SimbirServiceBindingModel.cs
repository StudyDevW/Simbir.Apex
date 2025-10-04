using DataModels.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace Contracts.BindingModels {
    public class SimbirServiceBindingModel : ISimbirService {
        public required string ServiceName { get; set; }
        public required IPEndPoint EndPointService { get; set; }

        public required int Id { get; set; }      
    }

    public class IPAddressConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return (objectType == typeof(IPAddress));
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
        public override bool CanConvert(Type objectType) {
            return (objectType == typeof(IPEndPoint));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            IPAddress address = jo["Address"]?.ToObject<IPAddress>(serializer) ?? throw new Exception("The converted operation failed");
            int port = jo["port"]?.ToObject<int>(serializer) ?? throw new Exception("The converted operation failed");
            return new IPEndPoint(address, port);
        }
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null) { return; }
            IPEndPoint ep = (IPEndPoint)value;
            JObject jo = new JObject();
            jo.Add("Address", JToken.FromObject(ep.Address, serializer));
            jo.Add("Port", ep.Port);
            jo.WriteTo(writer);
        }
    }
}