using UnityEngine;

namespace Bro.Client
{
    public interface IPool
    {
        GameObject Create();
        void Release(GameObject gameObject);
    }
}