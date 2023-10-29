using UnityEngine;

namespace Bro.Client
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)FindFirstObjectByType(typeof(T));
                }

                return _instance;
            }
        }

        void OnDestroy()
        {
            _instance = null;
        }
    }

}