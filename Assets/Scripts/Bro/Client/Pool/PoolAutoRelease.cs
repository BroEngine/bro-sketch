using System.Diagnostics;
using UnityEngine;

namespace Bro.Client
{
    public class PoolAutoRelease : MonoBehaviour
    {
        [SerializeField] private float _time = 3.0f;

        private IPool _pool;
        private readonly Stopwatch _timer = new Stopwatch();
        
        public void Setup(IPool pool)
        {
            _pool = pool;
        }

        private void OnEnable()
        {
            _timer.Restart();
        }

        private void Update()
        {
            if (_timer.ElapsedSeconds() >= _time)
            { 
                _timer.Reset();
                Release();
            }
        }

        private void Release()
        {
            if (_pool == null)
            {
                Destroy(this);
            }
            else
            {
                _timer.Reset();
                _pool.Release(gameObject);
            }
        }
    }
}