using Cysharp.Threading.Tasks;

namespace Bro.Client.Configs
{
    public interface IConfigStorage : IConfigProvider
    {
        string Identifier { get; }
        
        int Version { get; }
        
        UniTask InitializeAsync(string configData, int version, string identifier);

        string Dump();
    }
}