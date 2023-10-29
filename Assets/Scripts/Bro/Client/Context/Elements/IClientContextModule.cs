using Cysharp.Threading.Tasks;

namespace Bro.Client.Context
{
    public interface IClientContextModule : IClientContextElement
    {
        UniTask Load();
        UniTask Unload();
    }
}