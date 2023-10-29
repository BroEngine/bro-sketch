namespace Bro.Client.Configs
{
    public interface IConfigProvider
    {
    }

    public interface IConfigProvider<in TKey, out TValue> : IConfigProvider
    {
        TValue GetConfig(TKey key);
    }

    public interface IConfigProvider<out TValue> : IConfigProvider
    {
        TValue GetConfig();
    }
}