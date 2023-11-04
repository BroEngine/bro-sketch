using Bro.Client.Context;
using Cysharp.Threading.Tasks;

namespace Game.Client
{
    public class InventoryModule : IClientContextModule
    {
        private IClientContext _context;
        private LocalProfileModule _profile;
        private UserData _userData;

        public VehicleId VehicleId => _userData.VehicleId;
        
        public void Setup(IClientContext context)
        {
            _context = context;
            _profile = context.Get<LocalProfileModule>();
        }
        
        public async UniTask Load()
        {
            _userData = _profile.User;
            _context.AddDisposable(_context.Scheduler.ScheduleUpdate(OnUpdate));
            InitializeInventoryProfile();
        }
        
        public async UniTask Unload()
        {
            
        }
        
        private void OnUpdate(float dt)
        {
            
        }

        public void SetCurrentVehicleId(VehicleId vehicleId)
        {
            _userData.SetCurrentVehicleId(vehicleId);
        }

        private void InitializeInventoryProfile()
        {
            if (!_userData.IsInitialized)
            {
                _userData.SetHardCurrency(100); // todo
                _userData.SetSoftCurrency(200); // todo
                _userData.SetCurrentVehicleId(VehicleId.Vehicle01Marry); // todo
                _userData.SetInventoryInitialized();
            }
        }
    }
}