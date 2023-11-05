using Bro.Client.UI;
using Game.Client.Battle;
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
            var switchModule = _context.GetGlobal<BattleSwitchingModule>();
            switchModule.LoadBattle(new BattleConfig());
        }
    }
}