using UnityEngine.EventSystems;

namespace Bro.Client.UI
{
    public class CloseButton : ExtendedButton
    {
        private bool _invokeCloseAudioEvent;
        
        protected override void Awake()
        {
            base.Awake();
            _invokeCloseAudioEvent = _invokeAudioEvent;
            _invokeAudioEvent = false;
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            
            // if (_invokeCloseAudioEvent)
            {
                new UIAudioEvent(UIAudioType.ButtonClose).Launch();
            }
            
            var window = gameObject.GetComponentInParent<Window>();
            if (window != null)
            {
                window.UIModule.DirectlyHide(window);
            }
        }
    }
}