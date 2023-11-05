using Bro.Client.Context;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bro.Client.UI
{
    public class UIModule : IClientContextModule
    {
        private readonly UISettings _settings;
        private readonly UICreator _creator;
        private IClientContext _context;
        private UIManager _manager;
        
        public UIModule(UISettings settings, bool isPermanent, bool isEventSystem)
        {
            _settings = settings;
            _creator = new UICreator(isEventSystem, isPermanent);
        }
        
        public void Setup(IClientContext context)
        {
            _context = context;
        }
        
        public async UniTask Load()
        {
            _creator.CreateCanvas(_settings.Resolution, _settings.MatchFactor);
            _manager = new UIManager(_context, _creator.Root);
        }
        
        public async UniTask Unload()
        {
          
        }
        
        public void SwitchBlocker(bool active)
        {
            _creator.Blocker.transform.SetAsLastSibling();
            _creator.Blocker.SetActive(active);
        }

        
        /* implementation */
        
        
        public async UniTask AwaitShow<T>(IWindowArgs args = null) where T : Window
        {
            await _manager.AwaitShow<T>(args);
        }

        public T Show<T>(IWindowArgs args = null) where T : Window
        {
            return _manager.Show<T>(args);
        }

        public bool IsWindowOpened<T>() where T : Window
        {
            return _manager.IsWindowOpened<T>();
        }

        public bool IsWindowSpawned<T>() where T : Window
        {
            return _manager.IsWindowSpawned<T>();
        }

        public T GetWindow<T>() where T : Window
        {
            return _manager.GetWindow<T>();
        }

        public void Preload<T>(IWindowArgs args = null) where T : Window
        {
            _manager.Preload<T>(args);
        }

        public void Up<T>(bool auto = false) where T : Window
        {
            _manager.Up<T>(auto);
        }
        
        public void DirectlyHide<T>() where T : Window
        {
            _manager.DirectlyHide<T>();
        }
     
        public void DirectlyHide(Window window)
        {
            _manager.DirectlyHide(window);
        }
    }
}