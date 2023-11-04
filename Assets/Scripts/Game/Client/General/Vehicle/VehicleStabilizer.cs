using System.Collections.Generic;
using UnityEngine;

namespace Game.Client.Vehicle
{
    public class VehicleStabilizer : MonoBehaviour
    {
        [SerializeField] private float _airAngularDrag = 8.0f;
        [SerializeField] private float _airDownForce = 15000;
        
        private Rigidbody _rigidbody;
        private readonly List<WheelCollider> _wheelColliders = new List<WheelCollider>();
    
        private bool _isFlying;
        private bool _isColling;
        private float _defaultAngularDrag = 1.0f;
        private float _defaultDrag = 0.1f;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _defaultDrag = _rigidbody.drag;
            _defaultAngularDrag = _rigidbody.angularDrag;
            _wheelColliders.AddRange(transform.GetComponentsInChildren<WheelCollider>());
        }

        private void FixedUpdate()
        {
            Check();
        }

        private void Check()
        {
            var isGrounded = true;
            var isAnyGrounded = false;
            foreach (var wheel in _wheelColliders)
            {
                if (!wheel.isGrounded)
                {
                    isGrounded = false;
                }
                else
                {
                    isAnyGrounded = true;
                }
            }
            
            _isFlying = !isGrounded;
            _rigidbody.angularDrag = _isFlying ? _airAngularDrag : _defaultAngularDrag;

            if (_isFlying)
            {
                _rigidbody.AddForce(Vector3.down * _airDownForce, ForceMode.Force );
            }
        }
    }
}