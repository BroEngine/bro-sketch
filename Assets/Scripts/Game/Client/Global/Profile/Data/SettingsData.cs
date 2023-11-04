namespace Game.Client
{
    public class SettingsData : LocalProfileData
    {
        private readonly LocalProfileModule _module;
        private readonly LocalProfile.Settings _data;

        public bool VibrationEnabled => _data.VibrationEnabled;
        public bool MusicEnabled => _data.MusicEnabled;
        public bool SoundEnabled => _data.SoundEnabled;
        public bool AdsEnabled => _data.AdsEnabled;
        
        public SettingsData(LocalProfile.Settings data, LocalProfileModule module) : base(module)
        {
            _data = data;
            _module = module;
        }
        
        public void ToggleVibration()
        {
            _data.VibrationEnabled = !_data.VibrationEnabled;
            Save(false);
        } 
        
        public void ToggleMusic()
        {
            _data.MusicEnabled = !_data.MusicEnabled;
            Save(false);
        }
        
        public void ToggleSound()
        {
            _data.SoundEnabled = !_data.SoundEnabled;
            Save(false);
        }
        
        public void DisableAds()
        {
            _data.AdsEnabled = false;
            Save(true);
        }
        
        public override void Load()
        {
            
        }
    }
}