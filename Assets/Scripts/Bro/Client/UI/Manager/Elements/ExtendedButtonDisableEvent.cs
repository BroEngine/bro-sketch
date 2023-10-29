using Bro.Client;

namespace Bro.Client.UI
{
    public class ExtendedButtonDisableEvent : Event
    {
        public readonly float MaxTime;
        
        public ExtendedButtonDisableEvent(float maxTime = 10.0f)
        {
            MaxTime = maxTime;
        }
    }
}