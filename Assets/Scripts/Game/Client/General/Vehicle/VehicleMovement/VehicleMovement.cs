using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Client.Vehicle
{
    public class VehicleMovement
    {
        private class WheelMovement
        {
            public VehicleConfiguration.WheelLocation WheelLocation;
            public WheelCollider WheelCollider;
            public VehicleMovementConfig.WheelMovementConfig WheelMovementConfig;
        }

        private readonly List<WheelMovement> _wheels = new List<WheelMovement>();
        private VehicleMovementConfig _config;
        private Rigidbody _rigidbody;

        private VehicleDynamics _vehicleDynamics;
        private float _slipFactor;
        private float _stiffnessFactor;
        private float _speedRatio;

        public bool IsNitroActive { get; private set; }
        public bool IsForwardMoving { get; private set; }
        public float Acceleration { get; private set; }
        public float Braking { get; private set; }
        public float HandBraking { get; private set; }
        public float Steering { get; private set; }
        public bool IsAccelerating => Acceleration > float.Epsilon;
        public bool IsBraking => Braking > float.Epsilon;
        public bool IsHandBraking => HandBraking > float.Epsilon;
        public float SpeedRatio => _speedRatio;
        
        public VehicleMovement(VehicleConfiguration configuration, VehicleDynamics vehicleDynamics)
        {
            _vehicleDynamics = vehicleDynamics;

            _config = configuration.MovementConfig;

            _rigidbody = configuration.Rigidbody;
            _rigidbody.mass = _config.BodyMass;
            _rigidbody.centerOfMass = new Vector3(0f, 0f, _config.BodyMassOffset);

            foreach (var wheel in configuration.Wheels)
            {
                _wheels.Add(new WheelMovement()
                {
                    WheelLocation = wheel.WheelLocation,
                    WheelCollider = wheel.WheelCollider,
                    WheelMovementConfig = configuration.GetWheelMovementConfig(wheel.WheelLocation)
                });
            }
            

            foreach (var w in _wheels)
            {
                w.WheelCollider.forwardFriction = w.WheelMovementConfig.ForwardFriction.ToWheelFrictionCurve();
                w.WheelCollider.sidewaysFriction = w.WheelMovementConfig.SidewaysFriction.ToWheelFrictionCurve();
                w.WheelCollider.ConfigureVehicleSubsteps(5, 12, 15);
            }
        }
        
        
        public void Teleport(Vector3 position, Vector3 direction)
        {
            _rigidbody.rotation = Quaternion.LookRotation(direction);
            _rigidbody.position = position;
        }


        public void SetInput(ref VehicleInput input)
        {
            IsNitroActive = input.IsNitroActive;
            IsForwardMoving = input.IsForwardMoving;
              
            Braking = input.Braking;
            HandBraking = input.HandBraking;
            Steering = input.Steering;
            
            // as vehicle get closer to top speed, the acceleration decreases
            var maxSpeed = _config.GetMaxSpeed(IsForwardMoving, IsNitroActive);
            _speedRatio = _vehicleDynamics.Speed2d / maxSpeed;
            var speedFactor = (_vehicleDynamics.Speed2d / maxSpeed).Clamp01();
            var accelerationFactor = OutExpo(speedFactor);
            
            if (_speedRatio > _config.MaxSpeedAccelerationStopThreshold)
            {
                Acceleration = 0.0f;

                if (_speedRatio >_config.MaxSpeedBreakingStartThreshold)
                {
                    Braking = 1.0f; // temp
                }
            }
            else
            {
                Acceleration = accelerationFactor * input.Acceleration;
            }
         
            if (_config.TrackedSteering.IsEnabled)
            {
                Acceleration *= 1f - _config.TrackedSteering.DecelerationRatio * Steering;
            }
        }

        public void Simulate(float deltaTime)
        {
            foreach (var wheel in _wheels)
            {
                SimulateWheel(wheel, deltaTime);
            }
        }

        private void SimulateWheel(WheelMovement wheelMovement, float deltaTime)
        {
            var wheelMovementConfig = wheelMovement.WheelMovementConfig;
            var wheelCollider = wheelMovement.WheelCollider;

            SimulateSteeringOnTracks();
            SimulateSteeringOnWheels(wheelCollider, wheelMovementConfig.SteeringFactor);
            if (wheelCollider.isGrounded)
            {
                SimulateMotorForce(wheelCollider, wheelMovementConfig);
                SimulateBrakingForce(wheelCollider, wheelMovementConfig);
            }
            
            SimulateGrip(deltaTime, wheelCollider, wheelMovementConfig);
        }

        private void SimulateGrip(float deltaTime, WheelCollider wheelCollider, VehicleMovementConfig.WheelMovementConfig wheelMovementConfig)
        {
           
            if (IsHandBraking)
            {
                _slipFactor = (_slipFactor + deltaTime * _config.HandbrakeUsage.SlipFactorIncrease).Clamp01();
                _stiffnessFactor = Mathf.Lerp(_stiffnessFactor, 1f, deltaTime * _config.HandbrakeUsage.StiffnessFactorIncrease);
            }
            else
            {
                _slipFactor = (_slipFactor - deltaTime * _config.HandbrakeUsage.SlipFactorDecrease).Clamp01();
                _stiffnessFactor = (_stiffnessFactor - deltaTime * _config.HandbrakeUsage.StiffnessFactorDecrease).Clamp01();
            }

            var sidewaysExtremumSlip = Mathf.Lerp(wheelMovementConfig.SidewaysFriction.ExtremumSlip, _config.HandbrakeUsage.ExtremumSlipDuringBraking, _slipFactor);
            wheelCollider.SetSidewaysExtremumSlip(sidewaysExtremumSlip);
            var sidewaysStiffness = Mathf.Lerp(wheelMovementConfig.SidewaysFriction.Stiffness, _config.HandbrakeUsage.StiffnessDuringBraking, _stiffnessFactor * wheelMovementConfig.HandBrakeFactor);
            wheelCollider.SetSidewaysStiffness(sidewaysStiffness);
        }

        private void SimulateMotorForce(WheelCollider wheelCollider, VehicleMovementConfig.WheelMovementConfig wheelMovementConfig)
        {
            float directionSign = IsForwardMoving ? 1f : -1f;
            var accelerationForce = _config.GetAccelerationForce(IsForwardMoving, IsNitroActive);
            wheelCollider.motorTorque = wheelMovementConfig.MotorFactor * directionSign * Acceleration * accelerationForce;
        }

        private void SimulateBrakingForce(WheelCollider wheelCollider, VehicleMovementConfig.WheelMovementConfig wheelMovementConfig)
        {
            var handbrakeForce = wheelMovementConfig.HandBrakeFactor * HandBraking * _config.HandBrakeForce;
            var brakeForce = wheelMovementConfig.BrakeFactor * Braking * _config.BrakeForce;
            var totalBrakeForce = handbrakeForce + brakeForce;

            var contactGroundWorldPosition = wheelCollider.gameObject.transform.position - new Vector3(0f, wheelCollider.radius, 0f);
            // wheelCollider.GetWorldPose(out var pos, out var quaternion);
            // Vector3 eulerAngles = new Vector3(0f, quaternion.eulerAngles.y, 0f);
            
            var worldVelocity3d = _rigidbody.GetPointVelocity(contactGroundWorldPosition);
            var worldVelocity2d = new Vector2(worldVelocity3d.x, worldVelocity3d.z);
            
            var speedSqr = worldVelocity2d.sqrMagnitude;
            if (speedSqr > 0.01f)
            {
                var force2d = -worldVelocity2d.normalized * (totalBrakeForce * (speedSqr / 0.25f).Clamp01());
                var force3d = new Vector3(force2d.x, 0f, force2d.y);
                _rigidbody.AddForceAtPosition(force3d, contactGroundWorldPosition);
            }
        }

        private void SimulateSteeringOnWheels(WheelCollider wheel, float steeringFactor)
        {
            if (_config.WheeledSteering.IsEnabled)
            {
                var steeringAngle = Steering * _config.WheeledSteering.MaxSteeringAngle * steeringFactor;
                wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, steeringAngle, _config.WheeledSteering.SteeringSpeed);
            }
        }

        private void SimulateSteeringOnTracks()
        {
            if (_config.TrackedSteering.IsEnabled)
            {
                float directionSign = IsForwardMoving ? 1f : -1f;
                bool tooFastSteering = _vehicleDynamics.AbsAngularSpeed > _config.TrackedSteering.MaxAngularSpeed;
                if (!tooFastSteering)
                {
                    _rigidbody.AddTorque(new Vector3(0f, _config.TrackedSteering.TrackedSteeringForce * Steering * directionSign, 0f), ForceMode.Force);
                }
            }
        }
        
        private static float OutExpo(float x, float power = -8)
        {
            // https://easings.net/#easeOutExpo
            return x >= 1f ? 1f : 1f - (float)Math.Pow(2, power * (1f - x));
        }
    }
}