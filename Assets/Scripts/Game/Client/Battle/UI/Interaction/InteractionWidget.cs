using Bro.Client.Context;
using Bro.Client.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client.Battle.UI
{
    public class InteractionWidget : MonoBehaviour
    {
        [SerializeField] private Button _buttonExit;
        [SerializeField] private Button _buttonRestart;
        
        private IClientContext _context;
        private UIModule _uiModule;
        
        private void Awake()
        {
            _buttonExit.onClick.AddListener(OnButtonExit);
            _buttonRestart.onClick.AddListener(OnButtonRestart);
        }

        public void Setup(IClientContext context)
        {
            _context = context;
            _uiModule = _context.Get<UIModule>();
        }

        private void OnButtonRestart()
        {
            
        }

        private void OnButtonExit()
        {
            var switchModule = _context.GetGlobal<LobbySwitchingModule>();
            switchModule.LoadLobby();
        }
    }
}