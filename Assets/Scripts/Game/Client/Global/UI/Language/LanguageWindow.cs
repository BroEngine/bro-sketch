using System.Collections;
using System.Collections.Generic;
using Bro.Client;
using Bro.Client.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client.UI
{
    [Window(WindowItemType.Popup)]
    public class LanguageWindow : Window
    {
        [SerializeField] private GameObject _prefabItem;
        [SerializeField] private Button _buttonApply;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private Transform _scrollContainer;
        [SerializeField] private Transform _itemCurrent;
        [SerializeField] private Transform _itemOther;
        
        private LocalizationModule _localization;
        private LocalProfileModule _profile;
        private SettingsData _settings;
        private Language _language;

        private readonly List<LanguageWindowItem> _items = new List<LanguageWindowItem>();
        
        private void Awake()
        {
            _buttonApply.onClick.AddListener(OnButtonApply);
        }
        
        protected override void OnShow(IWindowArgs args)
        {
            _profile = _context.GetGlobal<LocalProfileModule>();
            _localization = _context.GetGlobal<LocalizationModule>();
            _settings = _profile.Settings;
            _language = _localization.Language;
            
            Configure();
            ReDraw(true);
            StartCoroutine(ToTop());
        }
        
        private IEnumerator ToTop()
        {
            for (var i = 0; i < _items.Count; ++i)
            {
                var item = _items[i];
                if (_language == item.Language)
                {
                    yield return null;
                    ScrollTo(item.gameObject);
                }
            }
        }

        private void Configure()
        {
            if (_items.Count == 0)
            {
                var languages = ConfigPath.AvailableLanguages;
                foreach (var language in languages)
                {
                    var o = Instantiate(_prefabItem, _scrollContainer);
                    var script = o.GetComponent<LanguageWindowItem>();
                    script.Setup(_localization, language, OnItemPressed);
                    _items.Add(script);
                }
            }
        }

        private void ReDraw(bool rearrange)
        {
            _itemCurrent.SetSiblingIndex(0);
            _itemOther.SetSiblingIndex(2);
            
            for (var i = 0; i < _items.Count; ++i)
            {
                var item = _items[i];
                var isSelected = _language == item.Language;
                item.SetSelected(isSelected);
              
                if (rearrange)
                {
                    item.gameObject.transform.SetSiblingIndex(isSelected ? 1 : i + 100);
                }
            }
        }

        private void OnItemPressed(Language languageType)
        {
            _language = languageType;
            ReDraw(false);
        }

        private void OnButtonApply()
        {
            _localization.SwitchLanguage(_language);
            DirectlyHide();
        }
        
        private void ScrollTo(GameObject target)
        {
            Canvas.ForceUpdateCanvases();
            _scroll.FocusOnItem(target.GetComponent<RectTransform>());
        }
    }
}