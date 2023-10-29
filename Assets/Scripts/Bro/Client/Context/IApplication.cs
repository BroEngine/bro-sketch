using Cysharp.Threading.Tasks;

namespace Bro.Client.Context
{   
    public interface IApplication
    {   
        IClientContext LocalContext { get; }
        IClientContext GlobalContext { get; }
        
        IScheduler GetScheduler(IClientContext context);
        UniTask SwitchContext(ClientContext context);
    }
}