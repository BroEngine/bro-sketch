#if UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_XBOXONE || UNITY_PS4 || UNITY_WEBGL || UNITY_WII
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Bro.Client.Context;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Bro.Client.UI
{
   
    public class UIManager
    {
        private readonly IClientContext _context;
        private readonly UIFactory _factory;
        
        private int _counterShow;
        private readonly WindowStack _windowStack = new WindowStack();
        private readonly Dictionary<Type, Window> _windows = new Dictionary<Type, Window>();
        private readonly Dictionary<Type, Action> _pendingWindowsShow = new Dictionary<Type, Action>();
        private readonly List<Type> _autoUpWindows = new List<Type>();
        private Window _activeWindow;
        
        public UIManager(IClientContext context, Transform root)
        {
            _context = context;
            _factory = new UIFactory(context, root);
        }
        
        /// <summary>
        /// Returns true when window is spawned from prefab 
        /// </summary>
        public bool IsWindowSpawned<T>() where T : Window
        {
            return _windows.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Returns true when widnow is currently showing
        /// </summary>
        public bool IsWindowOpened<T>() where T : Window
        {
            return IsWindowSpawned<T>() && GetWindow<T>().IsShowing();
        }  
        
        public bool IsWindowOpened(Type type) 
        {
            return _windows.ContainsKey(type) && GetWindow(type).IsShowing();
        }
        
        public Window GetWindow(Type type) 
        {
            if (_windows.ContainsKey(type))
            {
                return _windows[type] as Window;
            }

            Debug.LogError("window not spawned: " + type);
            return null;
        }    
        
        public T GetWindow<T>() where T : Window
        {
            var type = typeof(T);
            if (_windows.ContainsKey(type))
            {
                return _windows[type] as T;
            }

            Debug.LogError("window not spawned: " + type);
            return null;
        }    
        
        public void Preload<T>(IWindowArgs args = null) where T : Window
        {
            if (_windows.ContainsKey(typeof(T)))
            {
                return;
            }

            var openingWindow = CacheWindow<T>();
            if (openingWindow != null)
            {
                openingWindow.ShowWindow(args);
                openingWindow.HideWindow();
            }
        }

        public void Up<T>(bool auto) where T : Window
        {
            var itemToShow = CacheWindow<T>();
                 
            if (itemToShow == null)
            {
                Debug.LogError($"prefab for ui with type {typeof(T).Name} not found");
                return;
            }

            ++ _counterShow;
            itemToShow.gameObject.transform.SetSiblingIndex(_counterShow);

            if (auto)
            {
                _autoUpWindows.Remove(typeof(T));
                _autoUpWindows.Add(typeof(T));
            }
        }

        private void ReUp()
        {
            for (var i = _autoUpWindows.Count - 1; i >= 0; --i)
            {
                if (!IsWindowOpened(_autoUpWindows[i]))
                {
                    _autoUpWindows.RemoveAt(i);
                }
            }

            foreach (var type in _autoUpWindows)
            {
                ++ _counterShow;
                _windows[type].gameObject.transform.SetSiblingIndex(_counterShow);
            }
        }

        public async UniTask AwaitShow<T>(IWindowArgs args = null) where T : Window
        {
            var window = Show<T>(args);
            await UniTask.WaitUntil(() => window.CurrentState == Window.State.Shown);
        }
        
        public T Show<T>(IWindowArgs args = null) where T : Window
        {
            var itemToShow = CacheWindow<T>();
                 
            if (itemToShow == null)
            {
                Debug.LogError($"prefab for ui with type {typeof(T).Name} not found");
                return null;
            }

            ++ _counterShow;
            itemToShow.gameObject.transform.SetSiblingIndex(_counterShow);
            
            _pendingWindowsShow.Remove(typeof(T));

            switch (itemToShow.ItemType)
            {
                case Window.WindowItemType.Window:
                    ShowWindow(itemToShow, args);
                    break;
                case Window.WindowItemType.Popup:
                    ShowPopup(itemToShow, args);
                    break;
                case Window.WindowItemType.Widget:
                    itemToShow.ShowWindow(args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ReUp();

            return itemToShow;
        }

        private void ShowWindow(Window window, IWindowArgs args)
        {
            var prevWindow = _windowStack.Peek();

            if (_windowStack.Push(new WindowStackItem(window, args)))
            {
                _activeWindow?.HideWindow();

                prevWindow?.Window.HideWindow();

                _activeWindow = window;

                _activeWindow.ShowWindow(args);

                new WindowEvent(WindowEvent.ActionType.Showed, window).Launch();
            }
            else
            {
                window.Disable();
            }
        }

        private void ShowPopup(Window popup, IWindowArgs args = null)
        {
            var prevWindow = _windowStack.Peek();

            if (_windowStack.Push(new WindowStackItem(popup, args)))
            {
                if (prevWindow != null && prevWindow.Window.ItemType == Window.WindowItemType.Popup && prevWindow.Window.WindowPriority > popup.WindowPriority)
                {
                    prevWindow.Window.HideWindow();
                }

                popup.gameObject.transform.SetSiblingIndex(short.MaxValue);
                popup.ShowWindow(args);
                new WindowEvent(WindowEvent.ActionType.Showed, popup).Launch();
            }
            else
            {
                popup.Disable();
            }
        }

        public void Hide()
        {
            if (_windowStack.Count == 0)
            {
                return;
            }

            var windowToClose = _windowStack.Pop().Window;
            windowToClose.HideWindow();
            
            if (_activeWindow == windowToClose)
            {
                _activeWindow = null;
            }
            
            if (_windowStack.Count > 0)
            {
                var windowToOpen = _windowStack.Peek();

                windowToOpen.Window.ShowWindow(windowToOpen.Args, true);

                if (windowToOpen.Window.ItemType != Window.WindowItemType.Popup)
                {
                    _activeWindow = windowToOpen.Window;
                }
                else if (_activeWindow == null)
                {
                    var lastWindowInStack = _windowStack.GetLastWindowOfType(Window.WindowItemType.Window);

                    if (lastWindowInStack != null)
                    {
                        _activeWindow = lastWindowInStack.Window;
                        _activeWindow.ShowWindow(lastWindowInStack.Args, true);
                    }
                }
            }
            
            new WindowEvent(WindowEvent.ActionType.Hided, windowToClose).Launch();
        }

        public void HideAllWindow()
        {
            ///
            for (int i = 0; i < _windows.Count; ++i)
            {
                var window = _windows.Values.ElementAt(i);
                if (window.IsShowing())
                {
                    window.HideWindow();
                }
            }

            ResetData();
        }

        public void DestroyAllWindows()
        {
            for (int i = 0; i < _windows.Count; ++i)
            {
                var window = _windows.Values.ElementAt(i);
                if (window != null && window.gameObject != null)
                {
                    Object.Destroy(window.gameObject);
                }
            }

            _windows.Clear();
            ResetData();
        }

        void ResetData()
        {
            _windowStack.Clear();
            _activeWindow = null;
        }

        public void DestroyWindow<T>() where T : Window
        {
            for (int i = 0; i < _windows.Count; ++i)
            {
                var window = _windows.Values.ElementAt(i);
                if (window != null && window.gameObject != null && typeof(T) == window.GetType())
                {
                    Object.Destroy(window.gameObject);
                    _windows.Remove(typeof(T));
                    break;
                }
            }
        }
        
        public void DirectlyHide(Type type)
        {
            if (!_windows.ContainsKey(type))
            {
                Debug.Log("(not an error) Cant close window, case it not spawned: " + type.ToString());
            }
            else
            {
                var closingWindow = _windows[type];
                _windowStack.Remove(closingWindow);
                closingWindow.HideWindow();
                new WindowEvent(WindowEvent.ActionType.Hided, closingWindow).Launch();
            }
        }
        
        public void DirectlyHide(Window w)
        {
            var type = w.GetType();
            DirectlyHide(type);
        }

        public void DirectlyHide<T>() where T : Window
        {
            var type = typeof(T);
            DirectlyHide(type);
        }
        
        public async UniTask AwaitUntilHide<T>() where T : Window
        {
            var window = GetWindow<T>();
            if (window != null)
            {
                await UniTask.WaitUntil(() => window.CurrentState == Window.State.Closed);
            }
        }
        
        public async UniTask AwaitDirectlyHide<T>() where T : Window
        {
            var window = GetWindow<T>();
            if (window != null)
            {
                DirectlyHide<T>();
                await UniTask.WaitUntil(() => window.CurrentState == Window.State.Closed);
            }
        }
        
        /// <summary>
        /// Add window type to show pending
        /// </summary>
        public void AddWindowPendingShow<T>(IWindowArgs args = null) where T : Window
        {
            if (_windowStack.Peek().Window is T) return;

            Debug.Log($"ui component :: add pending window {typeof(T).Name}");

            _pendingWindowsShow[typeof(T)] = () => Show<T>(args);
        }

        public void ShowPendingWindows()
        {
            if (_pendingWindowsShow.Count == 0) return;

            var windowsToShow = new Queue<Type>(_pendingWindowsShow.Keys);

            while (windowsToShow.Count > 0)
            {
                _pendingWindowsShow[windowsToShow.Dequeue()]?.Invoke();
            }

            _pendingWindowsShow.Clear();
        }

        
        /// <summary>
        /// Instantiate window to cache or find cached
        /// </summary>
        /// <returns>New instanced window of cached</returns>
        private T CacheWindow<T>() where T : Window
        {
            var type = typeof(T);
            T result = null;

            if (!_windows.ContainsKey(type))
            {
                result = _factory.Create<T>();
                if (result != null)
                {
                    _windows.Add(type, result);
                }
            }
            else
            {
                result = _windows[type] as T;
            }

            return result;
        }
    }
}
#endif