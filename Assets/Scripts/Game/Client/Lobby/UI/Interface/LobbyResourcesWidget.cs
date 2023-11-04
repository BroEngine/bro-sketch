using Bro.Client.UI;
using Game.Client.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client.Lobby.UI
{
    [Window(WindowItemType.Widget)]
    public class LobbyResourcesWidget : Window
    {
        [SerializeField] private TextMeshProUGUI _labelName;
        [SerializeField] private TextMeshProUGUI _labelGold;

        [SerializeField] private Button _buttonName;
        [SerializeField] private Button _buttonSettings;

        private void Awake()
        {
            _buttonName.onClick.AddListener(OnButtonName);
            _buttonSettings.onClick.AddListener(OnButtonSettings);
        }

        protected override void OnShow(IWindowArgs args)
        {
            
        }

        private void OnButtonName()
        {
            
        }

        private void OnButtonSettings()
        {
            _context.Get<UIModule>().Show<SettingsWindow>();
        }
    }
}