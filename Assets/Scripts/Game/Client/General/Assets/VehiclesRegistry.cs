using Bro.Client;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Game.Client
{
   
    [AssetSettings("registry_vehicles", "Resources/Registries")]
    public class VehiclesRegistry : AssetSettings<VehiclesRegistry>
    {
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("Registries/Vehicles")]
        public static void Edit()
        {
            Instance = null;
            UnityEditor.Selection.activeObject = Instance;
            DirtyEditor();
        }
        #endif

        [SerializeField] private SerializableDictionaryBase<VehicleId, VehicleRegistryItem> _vehicles = new SerializableDictionaryBase<VehicleId, VehicleRegistryItem>();
    
        public VehicleRegistryItem GetVehicle(VehicleId vehicleId)
        {
            if (_vehicles.ContainsKey(vehicleId))
            {
                return _vehicles[vehicleId];
            }
            
            Debug.LogError($"no element for vehicle id = {vehicleId}");
            return default;
        }
    }
}