using Game.Client;
using TMPro;
using UnityEngine;

namespace Bro.Client.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TranslatableText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textElement;
        [SerializeField] private string _localizationKey;
        
        private LocalizationModule _localizationModule;
        private EventObserver<SwitchLanguageEvent> _eventObserver;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_textElement == null)
            {
                _textElement = GetComponent<TextMeshProUGUI>();
            }
        }
        #endif

        public void Awake()
        {
            _eventObserver = new EventObserver<SwitchLanguageEvent>(OnSwitchLanguage);
            ProcessTranslate();
        }

        private void OnSwitchLanguage(SwitchLanguageEvent e)
        {
            ProcessTranslate();
        }

        public void OnDestroy()
        {
            _eventObserver?.Unsubscribe();
            _eventObserver = null;
        }
        
        private void ProcessTranslate()
        {
            if (_localizationModule == null)
            {
                // todo - remove static ref
                _localizationModule = Game.Client.Game.Instance.GlobalContext.Get<LocalizationModule>(); 
            }
            
            _textElement.text = _localizationModule.Translate(_localizationKey);
        }
    }
}