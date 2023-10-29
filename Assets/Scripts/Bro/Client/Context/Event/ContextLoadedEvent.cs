namespace Bro.Client.Context
{
    public class ContextLoadedEvent : Event
    {
        public readonly IClientContext Context;

        public ContextLoadedEvent(IClientContext context)
        {
            Context = context;
        }
    }
}