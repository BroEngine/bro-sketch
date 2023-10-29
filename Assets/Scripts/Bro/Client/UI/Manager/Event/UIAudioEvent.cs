using Bro.Client;

namespace Bro.Client.UI
{
    public class UIAudioEvent : Event
    {
        public readonly UIAudioType Type;
        
        public UIAudioEvent(UIAudioType type)
        {
            Type = type;
        }
    }
}