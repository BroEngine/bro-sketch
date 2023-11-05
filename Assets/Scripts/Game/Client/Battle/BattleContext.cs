using Bro.Client.Context;
using Bro.Client.UI;
using Game.Client.Battle.Input;

namespace Game.Client.Battle
{
    public class BattleContext : ClientContext
    {
        public readonly BattleConfig Config;

        private readonly UIModule _uiModule = new UIModule(new UISettings(), false, false);
        private readonly CameraModule _cameraModule = new CameraModule();
        
        private readonly SettingsModule _settingsModule = new SettingsModule();
        private readonly HeroModule _heroModule = new HeroModule();
        private readonly InputModule _inputModule = new InputModule();
    
        private readonly LifecycleModule _lifecycleModule = new LifecycleModule();
        
        public BattleContext(IApplication application, BattleConfig config) : base(application)
        {
            Config = config;
        }
    }
}