using System;

namespace Game.Client
{
    public class UserData : LocalProfileData
    {
        private readonly LocalProfileModule _module;
        private readonly LocalProfile.User _data;
        
        public bool IsInitialized => _data.IsInitialized;
        public int SoftCurrency => _data.SoftCurrency;
        public int HardCurrency => _data.HardCurrency;
        public VehicleId VehicleId => _data.VehicleId;
        
        public UserData(LocalProfile.User data, LocalProfileModule module) : base(module)
        {
            _data = data;
            _module = module;
        }
        
        public override void Load()
        {
            
        }

        public void SetCurrentVehicleId(VehicleId vehicleId)
        {
            _data.VehicleId = vehicleId;
            Save(true);
        }

        public void SetInventoryInitialized()
        {
            _data.IsInitialized = true;
            Save(true);
        }

        public void SetSoftCurrency(int value)
        {
            _data.SoftCurrency = Math.Max(0, value);
            Save(true);
        } 
        
        public void SetHardCurrency(int value)
        {
            _data.HardCurrency = Math.Max(0, value);
            Save(true);
        }
    }
}