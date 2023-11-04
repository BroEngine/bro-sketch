using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using Bro;
using Bro.Client.Context;
using Bro.Client.Storage;

namespace Game.Client
{
    public class LocalProfileModule : IClientContextModule
    {
        private const long SavePeriod = 5000L;
        
        private IClientContext _context;
        private LocalizationModule _localizationModule;
        private readonly Stopwatch _saveTimer = new Stopwatch();
        private readonly LocalProfileStorage<LocalProfile> _localStorage = new LocalProfileStorage<LocalProfile>("profile", Config.Encryption);
        private readonly EventObserver<ApplicationFocusEvent> _focusObserver = new EventObserver<ApplicationFocusEvent>();
        private readonly List<LocalProfileData> _data = new List<LocalProfileData>();
        
        public string SaveJson => _localStorage.Json;
        
        public IClientContext Context => _context;
        public UserData User { get; private set; }
        public SettingsData Settings { get; private set; }
        public CampaignData Campaign { get; private set; }
        
        void IClientContextElement.Setup(IClientContext context)
        {
            _context = context;
            _localizationModule = _context.Get<LocalizationModule>();
        }

        async UniTask IClientContextModule.Load()
        {
            await _localStorage.LoadAsync();
            
            var model = _localStorage.Model;
            
            User = new UserData(model.UserData, this);
            Settings = new SettingsData(model.SettingsData, this);
            Campaign = new CampaignData(model.CampaignData, this);
            /* ... */
            
            _data.Add(User);
            _data.Add(Settings);
            _data.Add(Campaign);
            /* ... */
            
            foreach (var data in _data)
            {
                data.Load();
            }
            
            _context.AddDisposable(_context.Scheduler.ScheduleUpdate(OnUpdate));
            _focusObserver.Subscribe(OnApplicationFocusEvent);
        }

        async UniTask IClientContextModule.Unload()
        {
            _localStorage.Save(true);
        }

        public void DeleteAll()
        {
            _localStorage.Reset();
        }

        public void Save(bool force)
        {
            foreach (var data in _data)
            {
                data.OnSave();
            }
            
            if (force)
            {
                _saveTimer.Stop();
                _saveTimer.Reset();
                _localStorage.Save(true);
            }
            else
            {
                _saveTimer.Start();
            }
        }
        
        private void OnUpdate(float dt)
        {
            if (_saveTimer.ElapsedMilliseconds > SavePeriod)
            {
                Save(true);
            }
        }
        
        private void OnApplicationFocusEvent(ApplicationFocusEvent e)
        {
            if (!e.IsFocus)
            {
                Save(true);
            }
        }
    }
}