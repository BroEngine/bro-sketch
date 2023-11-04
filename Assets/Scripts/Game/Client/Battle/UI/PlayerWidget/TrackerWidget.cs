using UnityEngine;

namespace Game.Client.Battle.UI
{
    public class TrackerWidget : MonoBehaviour
    {
        private Camera _camera;
        private Transform _target;
        private RectTransform _rectTransform;
        
        public void SetupTrack(Transform target)
        {
            _camera = Camera.main;
            _target = target;
            _rectTransform = (RectTransform)transform;
            _rectTransform.position = new Vector2(Screen.width *  2, Screen.height * 2); // outside
        }

        public void StopTrack()
        {
            _target = null;
        }

        protected virtual void LateUpdate()
        {
            if (_camera == null || !_camera.enabled || _target == null)
            {
                return;
            }

            var worldPosition = _target.position;
            var screenPosition = _camera.WorldToScreenPoint(worldPosition);
            _rectTransform.position = screenPosition;
        }
    }
}