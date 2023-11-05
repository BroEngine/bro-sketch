using Bro.Client.Context;
using Bro.Client.UI;
using Game.Client.Battle;
using Game.Client.UI;
using Cysharp.Threading.Tasks;

namespace Game.Client
{
    public class BattleSwitchingModule : IClientContextModule
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

        public void LoadBattle(BattleConfig config)
        {
            if (!_isSwitching)
            {
                _isSwitching = true;
                _uiGlobalModule.SwitchBlocker(true);
                ProcessLoad(config).Forget();
            }
        }

        private async UniTask ProcessLoad(BattleConfig config)
        {
            await _uiGlobalModule.AwaitShow<LoadingSceneWindow>();
            var loadingWindow = _uiGlobalModule.GetWindow<LoadingSceneWindow>();
            
            loadingWindow.UpdateProgress(0.5f, 1.0f);
            
            await _context.GetApplication().SwitchToBattle(config);

            _isSwitching = false;
            
            /*
             * block and loading removal will take place in battle context
             */
        }
    }
}