namespace Bro.Client.Context
{
    public class ApplicationPauseEvent : Event
    {
        public readonly bool IsPause;

        public ApplicationPauseEvent(bool isPause)
        {
            IsPause = isPause;
        }
    }
}