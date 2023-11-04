using System;
using Bro.Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client.UI
{
    public class LanguageWindowItem : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private GameObject _objectSelected;

        public Language Language { get; private set; }
        private Action<Language> _callback;

        private void Awake()
        {
            _button.onClick.AddListener(OnButton);
        }

        public void Setup(LocalizationModule localizationModule, Language language, Action<Language> callback)
        {
            Language = language;
            _callback = callback;
            _label.text = localizationModule.Translate($"[language_{language.GetDescription()}]");
        }

        public void SetSelected(bool selected)
        {
            _objectSelected.SetActive(selected);
        }

        private void OnButton()
        {
            _callback?.Invoke(Language);
        }
    }
}