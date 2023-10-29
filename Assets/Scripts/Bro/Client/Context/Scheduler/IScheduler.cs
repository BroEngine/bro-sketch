using System;

namespace Bro.Client.Context
{
    public interface IScheduler : IDisposable
    {
        IDisposable ScheduleFixedUpdate(Action<float> update);

        IDisposable ScheduleLateUpdate(Action<float> update);

        IDisposable ScheduleUpdate(Action<float> update);
    }
}