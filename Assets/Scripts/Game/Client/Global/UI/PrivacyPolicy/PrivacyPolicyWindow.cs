using System;
using Bro.Client.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client.UI
{
    public class PrivacyPolicyWindowArgs : IWindowArgs
    {
        public readonly Action Callback;
        
        public PrivacyPolicyWindowArgs(Action callback)
        {
            Callback = callback;
        }
    }
    
    [Window(WindowItemType.Window)]
    public class PrivacyPolicyWindow : Window
    {
        [SerializeField] private Button _buttonAccept;
        [SerializeField] private Button _buttonLink;

        private Action _callback;
        
        private void Awake()
        {
            _buttonAccept.onClick.AddListener(OnButtonAccept);
            _buttonLink.onClick.AddListener(OnButtonLink);
        }
        
        protected override void OnShow(IWindowArgs args)
        {
            var arguments = (PrivacyPolicyWindowArgs) args;
            _callback = arguments?.Callback;
        }
        
        private void OnButtonLink()
        {
            Application.OpenURL (Config.PrivacyPolicy);          
        }

        private void OnButtonAccept()
        {
            _callback?.Invoke();
            DirectlyHide();
        }
    }
}