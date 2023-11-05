using Bro.Client.Context;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Client.Battle
{
    public class CameraModule : IClientContextModule
    {
        private IClientContext _context;
        private CameraBehaviour _camera;

        private Vector3 _position;
        private Quaternion _rotation;
        /* _data */
        
        public void Setup(IClientContext context)
        {
            _context = context;
        }
        
        public async UniTask Load()
        {
            _camera = Object.FindObjectOfType<CameraBehaviour>(true);
            _context.AddDisposable(_context.Scheduler.ScheduleLateUpdate(OnLateUpdate));
            Debug.Assert(_camera != null);
        }
        
        public async UniTask Unload()
        {
        
        }
        
        private void OnLateUpdate(float dt)
        {
            _camera.Move(_position, _rotation);
        }
        
        public void Move(Vector3 position, Quaternion rotation)
        {
            _position = position;
            _rotation = rotation;
        }
    }
}