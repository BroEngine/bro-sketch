using Bro.Client.Configs;
using Bro.Client.Context;
using Bro.Client.UI;

namespace Game.Client
{
    public class GlobalContext : ClientContext
    {
        private readonly LocalProfileModule _localProfileModule = new LocalProfileModule();
        private readonly LocalizationModule _localizationModule = new LocalizationModule();
        private readonly ConfigModule _configModule = new ConfigModule();
        private readonly InventoryModule _inventoryModule = new InventoryModule();
        
        private readonly UIModule _uiModule = new UIModule(new UISettings(), true, true);
        private readonly AudioModule _audioModule = new AudioModule();
        
        private readonly BattleSwitchingModule _battleSwitchingModule = new BattleSwitchingModule();
        private readonly LobbySwitchingModule _lobbySwitchingModule = new LobbySwitchingModule();
        
        public GlobalContext(IApplication application) : base(application)
        {
        }
    }
}