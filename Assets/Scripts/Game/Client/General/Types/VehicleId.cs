using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Game.Client
{
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
    public enum VehicleId
    {
        [EnumMember(Value = "undefined")] Undefined,
        [EnumMember(Value = "vehicle_01_marry")] Vehicle01Marry
    }
}