using Bro.Client.Context;
using Game.Client.Vehicle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Client.Battle
{
    public class HeroModule : IClientContextModule
    {
        private IClientContext _context;
        private CameraModule _cameraModule;
        private SettingsModule _settingsModule;
        private InventoryModule _inventoryModule;
        
        private HeroBehaviour _hero;
        private VehicleBehaviour _vehicle;
        private IVehicleInputProvider _inputProvider;
        
        public HeroBehaviour Hero => _hero;
        public VehicleBehaviour Vehicle => _vehicle;
        
        public void Setup(IClientContext context)
        {
            _context = context;
            _cameraModule = context.Get<CameraModule>();
            _settingsModule = context.Get<SettingsModule>();
            _inventoryModule = context.GetGlobal<InventoryModule>();
        }
        
        public async UniTask Load()
        {
            var vehicleId = _inventoryModule.VehicleId;
            var vehicleDescription = VehiclesRegistry.Instance.GetVehicle(vehicleId);
            var prefab = vehicleDescription.Prefab;

            if (prefab == null)
            {
                Debug.LogError($"hero prefab is null ({vehicleId})");
            }
            
            var vehicleObject = Object.Instantiate(prefab);
            
            _vehicle = vehicleObject.GetComponent<VehicleBehaviour>();
            _hero = vehicleObject.GetComponent<HeroBehaviour>();
            
            Debug.Assert(_vehicle != null);
            Debug.Assert(_hero != null);
          
            _inputProvider = new VehicleInputProvider(_vehicle, _context);
         
            // todo
            // _hero.Setup();
            _vehicle.Setup();
            _vehicle.SetInputProvider(_inputProvider);
            
            _context.AddDisposable(_context.Scheduler.ScheduleUpdate(OnUpdate));
        }
        
        public async UniTask Unload()
        {
            
        }
        
        private void OnUpdate(float dt)
        {
            // todo move to camera module
            _cameraModule.Move(_vehicle.transform.position, _vehicle.transform.rotation);
        }
    }
}