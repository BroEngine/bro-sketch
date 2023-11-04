using Bro.Client.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client.Lobby.UI
{
    [Window(WindowItemType.Widget)]
    public class LobbyInterfaceWidget : Window
    {
        [SerializeField] private Button _buttonStart;
        
        private void Awake()
        {
            _buttonStart.onClick.AddListener(OnButtonStart);
        }

        protected override void OnShow(IWindowArgs args)
        {
          
        }
        
        private void OnButtonStart()
        {
            // todo load battle
        }
    }
}