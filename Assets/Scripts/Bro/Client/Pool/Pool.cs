using System.Collections.Generic;
using UnityEngine;

namespace Bro.Client
{
    public class Pool<T> : IPool where T : MonoBehaviour
    {
        private readonly GameObject _prefab;
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly Dictionary<GameObject, T> _hash = new Dictionary<GameObject, T>();
        private readonly Transform _root;
        
        public Pool(GameObject prefab, int preloadSize, Transform root = null)
        {
            _root = root;
            _prefab = prefab;
            for (var i = 0; i < preloadSize; ++i)
            {
                _queue.Enqueue(Instantiate(_root));
            }
        }

        public T Create(Vector3 position, Quaternion rotation, Transform root)
        {
            var item = Create();

            item.transform.parent = root;
            item.transform.position = position;
            item.transform.rotation = rotation;
            
            return item;
        }
        
        public T Create()
        {
            if (_queue.Count > 0)
            {
                var oldItem = _queue.Dequeue();
                oldItem.gameObject.SetActive(true);
                return oldItem;
            }

            var newItem = Instantiate(_root);
            newItem.gameObject.SetActive(true);
            return newItem;
        }

        GameObject IPool.Create()
        {
            return Create().gameObject;
        }
        
        private T Instantiate(Transform root)
        {
            var gameObject = Object.Instantiate(_prefab, root);
            var script = gameObject.GetComponent<T>();
            gameObject.SetActive(false);
            _hash[gameObject] = script;

            PoolAutoRelease a;
            if (gameObject.TryGetComponent<PoolAutoRelease>(out a))
            {
                a.Setup(this);
            }
            
            return script;
        }
        
        public void Release(T script)
        {
            Release(script.gameObject);
        }
        
        public void Release(GameObject gameObject)
        {
            if (!_hash.ContainsKey(gameObject))
            {
                Debug.LogError("the object was not created through this pool");
                return;
            }

            var script = _hash[gameObject];
            gameObject.SetActive(false);
            _queue.Enqueue(script);   
        }
    }
}