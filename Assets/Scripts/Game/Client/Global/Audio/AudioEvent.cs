using Bro;
namespace Game.Client
{
    public class AudioEvent : Event
    {
        public readonly AudioType AudioType;
        public readonly bool IsLoop;
        
        public AudioEvent(AudioType type, bool loop = false)
        {
            IsLoop = loop;
            AudioType = type;
        }
    }
}