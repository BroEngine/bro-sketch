namespace Bro
{
    public interface IEventObserverAggregator
    {
        void Invoke(IEvent e);
        void Add(IEventObserver observer);
        void Remove(IEventObserver observer);
    }
}