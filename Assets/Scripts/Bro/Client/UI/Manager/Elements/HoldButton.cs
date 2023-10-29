using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bro.Client.UI
{
    public class HoldButton : Button
    {
        public bool IsHolding { get; private set; }

        public override void OnPointerDown(PointerEventData eventData)
        {
            IsHolding = true;
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            IsHolding = false;
            base.OnPointerUp(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            
            base.OnPointerExit(eventData);
        }
    }
}