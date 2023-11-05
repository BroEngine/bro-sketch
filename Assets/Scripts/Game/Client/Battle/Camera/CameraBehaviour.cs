using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Client.Battle
{
    public class CameraBehaviour : MonoBehaviour
    {
        [Space]
        [Tooltip("at what threshold the angle of camera will be changed instantly")]
        [SerializeField] private float _cameraAngleThreshold = 45f;
        [Tooltip("at what threshold the position of camera will be changed instantly")]
        [SerializeField] private float _cameraDistanceThreshold = 20.0f;
        
        [Space]
        [SerializeField] [Required] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Transform _transformRoot;

        [SerializeField] private Transform _transformPosition;
        [SerializeField] private Transform _transformAim;
        
        private Quaternion _rotation;
        private Vector3 _position;
        
        public void Move(Vector3 position, Quaternion rotation)
        {
            CheckThreshold(position, rotation);
            _position = position;
            _rotation = rotation;

            _transformPosition.position = _position;
        }
        
        private void CheckThreshold(Vector3 position, Quaternion rotation)
        {
            return;
            var angleDelta = Quaternion.Angle(rotation, _rotation);
            var positionDelta = Vector3.Distance(position, _position);

            if (angleDelta > _cameraAngleThreshold) 
            {
                _virtualCamera.PreviousStateIsValid = false;
            }

            if (positionDelta > _cameraDistanceThreshold) 
            {
                _virtualCamera.PreviousStateIsValid = false;
            }
        }
    }
}