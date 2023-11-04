using Bro.Client.Context;
using Bro.Client.UI;
using Game.Client.Lobby.UI;
using Cysharp.Threading.Tasks;

namespace Game.Client.Lobby
{
    public  class LobbyLifecycleModule : IClientContextModule
    {
        private IClientContext _context;
        private InventoryModule _inventoryModule;
        private UIModule _uiModule;
     
        private LobbyInterfaceWidget _interfaceWidget;
        
        public void Setup(IClientContext context)
        {
            _context = context;
            _uiModule = _context.Get<UIModule>();
            _inventoryModule = _context.GetGlobal<InventoryModule>();
        }
        
        public async UniTask Load()
        {
            await _uiModule.AwaitShow<LobbyInterfaceWidget>();
            await _uiModule.AwaitShow<LobbyResourcesWidget>();
            
            new LobbyLoadedEvent().Launch();
        }
        
        public async UniTask Unload()
        {
          
        }
    }
}