using Bro.Client.Context;
using Cysharp.Threading.Tasks;


namespace Game.Client.Battle
{
    public class SettingsModule : IClientContextModule
    {
        private IClientContext _context;
        private InventoryModule _inventory;
        private VehicleId _vehicleId;
        private VehicleConfigStorage _vehicleConfigStorage;
        private VehicleConfig _vehicleConfig;
        
        public void Setup(IClientContext context)
        {
            _context = context;
            _inventory = _context.GetGlobal<InventoryModule>();
            _vehicleId = _inventory.VehicleId;
            _vehicleConfigStorage = context.GetVehicleConfigStorage();
            _vehicleConfig = _vehicleConfigStorage.GetConfig(_vehicleId);
            
            // todo config -> settings
            // todo apply
            // todo upgrade
        }
        
        public async UniTask Load()
        {
               
        }
        
        public async UniTask Unload()
        {
        
        }
    }
}