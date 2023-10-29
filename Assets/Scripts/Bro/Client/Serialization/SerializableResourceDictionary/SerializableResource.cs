using Unity.VisualScripting;

namespace Bro.Client
{
    public abstract class SerializableResource
    {
        public abstract void Add();
        public abstract bool IsValidKey(string key);
    }
}