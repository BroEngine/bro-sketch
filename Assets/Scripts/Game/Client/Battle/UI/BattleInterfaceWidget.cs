using Bro.Client.Context;
using Bro.Client.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Client.Battle.UI
{
    [Window(WindowItemType.Widget)]
    public class BattleInterfaceWidget : Window
    {
        [SerializeField] [Required] private PlayerWidget _playerWidget;
        [SerializeField] [Required] private InputWidget _inputWidget;
        [SerializeField] [Required] private InteractionWidget _interactionWidget;

        protected override void OnSetup(IClientContext context)
        {
            base.OnSetup(context);
            
            _playerWidget.Setup(context);
            _inputWidget.Setup(context);
            _interactionWidget.Setup(context);
        }

        protected override void OnShow(IWindowArgs args)
        {
            
        }
    }
}