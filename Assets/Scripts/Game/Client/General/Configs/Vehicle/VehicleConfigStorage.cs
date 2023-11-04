using System.Collections.Generic;
using Bro.Client.Configs;

namespace Game.Client
{
    public class VehicleConfigStorage : DictionaryConfigStorage<VehicleId, VehicleConfig>
    {
        public IReadOnlyList<VehicleId> GetAvailableVehicles()
        {
            var result = new List<VehicleId>();
            foreach (var pair in Data)
            {
                result.Add(pair.Key);
            }
            return result;
        }
        
        public void Debug()
        {
            foreach (var pair in Data)
            {
                UnityEngine.Debug.LogError($"[{pair.Key}][{pair.Value.VehicleId} / {pair.Value.Health}]");
            }
        }
    }
}