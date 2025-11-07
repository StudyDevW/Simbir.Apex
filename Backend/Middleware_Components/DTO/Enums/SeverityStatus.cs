using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware_Components.DTO.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SeverityStatus
    {
        [JsonProperty("low")]
        LOW,
        [JsonProperty("medium")]
        MEDIUM,
        [JsonProperty("high")]
        HIGH
    }
}
