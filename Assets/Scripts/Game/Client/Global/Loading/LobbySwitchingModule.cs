using System;
using Bro.Client.Context;
using Bro.Client.UI;
using Game.Client.UI;
using Cysharp.Threading.Tasks;

namespace Game.Client
{
    public class LobbySwitchingModule : IClientContextModule
    {
        private IClientContext _context;
        private UIModule _uiGlobalModule; // global
        private bool _isSwitching = false;
        
        public void Setup(IClientContext context)
        {
            _context = context;
            _uiGlobalModule = context.Get<UIModule>();
        }
        
        public async UniTask Load()
        {
            
        }
        
        public async UniTask Unload()
        {
           
        }

        public void LoadLobby()
        {
            if (!_isSwitching)
            {
                _isSwitching = true;
                _uiGlobalModule.SwitchBlocker(true);
                ProcessLoad().Forget();
            }
        }
        
        private async UniTask ProcessLoad()
        {
            await _uiGlobalModule.AwaitShow<LoadingSceneWindow>();
            var loadingWindow = _uiGlobalModule.GetWindow<LoadingSceneWindow>();
            
            loadingWindow.UpdateProgress(0.9f, 1.0f);
            
            await _context.GetApplication().SwitchToLobby();
            
            loadingWindow.UpdateProgress(1.0f, 0.1f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            
            _uiGlobalModule.DirectlyHide<LoadingSceneWindow>();
            _uiGlobalModule.SwitchBlocker(false);
            
            _isSwitching = false;
        }
    }
}