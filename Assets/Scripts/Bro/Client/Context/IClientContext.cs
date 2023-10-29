using System;
using Cysharp.Threading.Tasks;

namespace Bro.Client.Context
{
    public interface IClientContext 
    {   
        bool IsLoaded { get; }
        IScheduler Scheduler { get; }
        IApplication Application { get; }
        UniTask Load();
        UniTask Unload();
        
        T Get<T>() where T : class, IClientContextElement;
        
        void AddDisposable(IDisposable disposable);
    }
}