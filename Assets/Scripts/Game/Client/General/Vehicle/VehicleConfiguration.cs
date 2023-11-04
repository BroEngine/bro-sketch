using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Client.Vehicle
{
    public class VehicleConfiguration : MonoBehaviour
    {
        public enum WheelLocation
        {
            FrontLeft,
            FrontRight,
            RearLeft,
            RearRight,
        }

        [Serializable]
        public class WheelConfiguration
        {
            public WheelLocation WheelLocation;
            public WheelCollider WheelCollider;
            public Transform Suspension;
            public Transform WheelTransform;
            public TrailRenderer SkidMarkRenderer;
            public ParticleSystem SkidSmokeParticles;
        }


        [SerializeField] private VehicleMovementConfig _movementConfig;
        [SerializeField] private VehicleVisualConfig _visualConfig;
        [SerializeField] private WheelConfiguration[] _wheels;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _bodyCollider;
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private GameObject _nitroEffect;
        
        public VehicleMovementConfig MovementConfig => _movementConfig;
        public VehicleVisualConfig VisualConfig => _visualConfig;
        public WheelConfiguration[] Wheels => _wheels;
        public Rigidbody Rigidbody => _rigidbody;
        public Collider BodyCollider => _bodyCollider;
        public Transform BodyTransform => _bodyTransform;
        public GameObject NitroEffect => _nitroEffect;
        
        public List<WheelCollider> FrontWheelColliders
        {
            get
            {
                List<WheelCollider> result = new List<WheelCollider>(2);
                foreach (var wheel in _wheels)
                {
                    if (wheel.WheelLocation.IsFront())
                    {
                        result.Add(wheel.WheelCollider);
                    }
                }

                return result;
            }
        }

        public List<WheelCollider> RearWheelColliders
        {
            get
            {
                List<WheelCollider> result = new List<WheelCollider>(2);
                foreach (var wheel in _wheels)
                {
                    if (wheel.WheelLocation.IsRear())
                    {
                        result.Add(wheel.WheelCollider);
                    }
                }

                return result;
            }
        }

        public VehicleMovementConfig.WheelMovementConfig GetWheelMovementConfig(WheelLocation location)
        {
            return location.IsFront() ? _movementConfig.FrontWheelsMovement : _movementConfig.RearWheelsMovement;
        }
    }
}