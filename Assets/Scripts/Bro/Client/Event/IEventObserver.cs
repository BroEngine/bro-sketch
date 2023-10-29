using System;

namespace Bro
{
    public interface IEventObserver : IDisposable
    {
        int EventId { get; }
        bool OnEvent(IEvent e);
    }
}