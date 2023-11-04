using System;
using Bro.Client.Context;
using Bro.Client.UI;
using Cysharp.Threading.Tasks;
using Game.Client.Battle.UI;
using Game.Client.UI;

namespace Game.Client.Battle
{
    public class LifecycleModule : IClientContextModule
    {
        private IClientContext _context;
        private UIModule _uiGlobalModule;
        private UIModule _uiModule;
        
        private HeroModule _heroModule;
        
        public void Setup(IClientContext context)
        {
            _context = context;
        
            _heroModule = _context.Get<HeroModule>();
            _uiModule = _context.Get<UIModule>();
            _uiGlobalModule = _context.GetGlobal<UIModule>();
        }
        
        public async UniTask Load()
        {
            var loadingWindow = _uiGlobalModule.GetWindow<LoadingSceneWindow>();
            loadingWindow.UpdateProgress(1.0f, 1.0f);
            
            _uiGlobalModule.DirectlyHide<LoadingSceneWindow>();
            _uiGlobalModule.SwitchBlocker(false);

            await _uiModule.AwaitShow<BattleInterfaceWidget>();

            new BattleLoadedEvent().Launch();
        }
        
        public async UniTask Unload()
        {
          
        }
    }
}