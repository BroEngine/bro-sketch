#if ( UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_XBOXONE || UNITY_PS4 || UNITY_WEBGL || UNITY_WII)

using UnityEngine;
using System;
using Bro.Client.Context;
using Cysharp.Threading.Tasks;

namespace Bro.Client.UI
{
    public abstract class Window : BaseWindow
    {
        protected IClientContext _context;
        
        private Animator _unityAnimator;
        private TweenAnimator _tweenAnimator;
        
        private readonly int _windowShowAnimation = Animator.StringToHash("Show");
        private readonly int _windowHideAnimation = Animator.StringToHash("Hide");
        
        public WindowItemType ItemType { get; set; }
        public IClientContext Context => _context;
        public UIModule UIModule => _context.Get<UIModule>();

        public enum State
        {
            Closed,
            Closing,
            Shown,
            Showing
        }

        public enum WindowItemType
        {
            Window,
            Popup,
            Widget
        }

        [SerializeField] private int _windowPriority = 0;
        [SerializeField] private bool _invokeAudioEvent = true;
        [SerializeField] private bool _customAnimation = false;
        
        private State _currentState;
        
        public int WindowPriority => _windowPriority;
        public State CurrentState => _currentState;

        public void Setup(IClientContext context, WindowItemType type)
        {
            _context = context;
            ItemType = type;
            OnSetup(context);
        }

        public void ShowWindow(IWindowArgs args = null, bool fromStack = false)
        {
            SetupComponents();
            Show().Forget();
            OnShow(args);
            
            if (_invokeAudioEvent)
            {
                new UIAudioEvent(UIAudioType.WindowShow).Launch();;
            }
        }

        public void HideWindow()
        {
            Hide();

            if (_invokeAudioEvent)
            {
                new UIAudioEvent(UIAudioType.WindowHide).Launch();
            }
        }
       
        public void Disable()
        {
            gameObject.SetActive(false);
            _currentState = State.Closed;
        }

        protected virtual void OnDestroy()
        {
            if (gameObject.activeSelf)
            {
                OnHideStart();
                OnHideEnd();
            }
        }

        private async UniTask Show()
        {
            UIModule.SwitchBlocker(true);
            
            _currentState = State.Showing;
            gameObject.SetActive(true);
            
            if (_customAnimation)
            {
                await AnimateShow();
            }
            else if (_tweenAnimator != null)
            {
                await _tweenAnimator.Show();
            }
            
            _currentState = State.Shown;

            OnShown();
            
            UIModule.SwitchBlocker(false);
        }

        private async UniTask Hide()
        {
            if (_currentState == State.Closing)
            {
                return;
            }

            UIModule.SwitchBlocker(true);
         
            await OnHideStart();
            
            if (_currentState == State.Showing || _currentState == State.Shown)
            {
                _currentState = State.Closing;
                
                if (_customAnimation)
                {
                    await AnimateHide();
                }
                if (_tweenAnimator != null)
                {
                    await _tweenAnimator.Hide();
                }
            }
         
            await OnHideEnd();
            
            UIModule.SwitchBlocker(false);
            Disable();
        }
        
        private async UniTask AnimateShow()
        {
            if (_unityAnimator != null)
            {
                _unityAnimator.Play(_windowShowAnimation);
                var waitTime = _unityAnimator.GetCurrentAnimatorStateInfo(0).length;
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
            }
            else
            {
                Debug.LogError("window :: cannot find animator for custom animation");
            }
        }

        private async UniTask AnimateHide()
        {
            if (_unityAnimator != null)
            {
                _unityAnimator.Play(_windowHideAnimation);
                var waitTime = _unityAnimator.GetCurrentAnimatorStateInfo(0).length;
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
            }
        }
        
        private void SetupComponents()
        {
            _unityAnimator = GetComponent<Animator>();
            _tweenAnimator = GetComponent<TweenAnimator>();
        }
        
        public bool IsShowing()
        {
            return gameObject.activeInHierarchy && (_currentState != State.Closed && _currentState != State.Closing);
        }

        protected void DirectlyHide()
        {
            _context.Get<UIModule>().DirectlyHide(this);
        }
    }
}

#endif