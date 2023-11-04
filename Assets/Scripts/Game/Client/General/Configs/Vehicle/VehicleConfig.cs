using System;
using Bro.Client.Json;
using Newtonsoft.Json;

namespace Game.Client
{
    [JsonType(typeName: "vehicle_config")]
    [Serializable]
    public class VehicleConfig
    {
        [JsonProperty("vehicle_id")] public VehicleId VehicleId;
        [JsonProperty("health")] public int Health;
        [JsonProperty("max_speed")] public int MaxSpeed;
        [JsonProperty("acceleration")] public int Acceleration;
    }
}