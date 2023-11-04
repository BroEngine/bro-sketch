using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Client.Vehicle
{
    public class VehicleVisual
    {
        [Serializable]
        private class WheelVisual
        {
            public WheelCollider WheelCollider;
            public Transform WheelTransform;
            public SkidEffect SkidEffect;
            public bool IsInvolvedInHandBraking;
            public bool IsInvolvedInAcceleration;
        }

        private VehicleConfiguration _vehicleConfiguration;
        private VehicleDynamics _vehicleDynamics;
        private VehicleMovement _vehicleMovement;
        private VehicleVisualConfig _visualConfig;
        private readonly List<WheelVisual> _wheels = new List<WheelVisual>();
        private Transform _body;

        public VehicleVisual(VehicleConfiguration configuration, VehicleDynamics dynamics, VehicleMovement movement)
        {
            _vehicleConfiguration = configuration;
            _vehicleMovement = movement;
            _vehicleDynamics = dynamics;
            _visualConfig = configuration.VisualConfig;
            foreach (var wheelConfiguration in configuration.Wheels)
            {
                var wheelMovementConfig = configuration.GetWheelMovementConfig(wheelConfiguration.WheelLocation);
                _wheels.Add(new WheelVisual()
                {
                    WheelCollider = wheelConfiguration.WheelCollider,
                    WheelTransform = wheelConfiguration.WheelTransform,
                    SkidEffect = new SkidEffect(wheelConfiguration.SkidMarkRenderer, wheelConfiguration.SkidSmokeParticles),
                    IsInvolvedInHandBraking = wheelMovementConfig.HandBrakeFactor > 0f,
                    IsInvolvedInAcceleration = wheelMovementConfig.MotorFactor > 0f,
                });
            }

            _body = configuration.BodyTransform;
        }

        public void Simulate(float deltaTime)
        {
            UpdateMoveIndicators(deltaTime);
            UpdateWheelsVisuals();
            UpdateSuspensionVisuals(deltaTime);
            _vehicleConfiguration.NitroEffect.SetActive(_vehicleMovement.IsNitroActive);
        }

        private void UpdateMoveIndicators(float deltaTime)
        {
            // bool isAccelerating = _vehicleMovement.IsAccelerating;
            // bool isForwardMovement = _vehicleMovement.IsForwardMoving;
            // var forwardIndicator = _vehicleConfiguration.ForwardMoveMarker;
            // var backwardIndicator = _vehicleConfiguration.BackwardMoveMarker;
            //
            // if (isAccelerating)
            // {
            //     forwardIndicator.gameObject.SetActive(isForwardMovement);
            //     var forwardY = Mathf.Lerp( Angle.Normalize(forwardIndicator.localEulerAngles.y), Mathf.Pow(_vehicleMovement.Steering, 3f) * 80f, deltaTime * 10f);
            //     forwardIndicator.localEulerAngles = forwardIndicator.localEulerAngles.SetY(forwardY);
            //     backwardIndicator.gameObject.SetActive(!isForwardMovement);
            //     var backwardY =  Mathf.Lerp( Angle.Normalize(backwardIndicator.localEulerAngles.y), -Mathf.Pow(_vehicleMovement.Steering, 3f) * 80f, deltaTime * 10f);
            //     backwardIndicator.localEulerAngles = backwardIndicator.localEulerAngles.SetY(backwardY);
            // }
            // else
            // {
            //     forwardIndicator.gameObject.SetActive(false);
            //     backwardIndicator.gameObject.SetActive(false);
            // }
        }

        private void UpdateSuspensionVisuals(float deltaTime)
        {
            var eulerAngles = _body.localEulerAngles;

            var sidewaysLerpValue = Mathf.InverseLerp(0f, _visualConfig.MaxAccelerationSideways, Math.Abs(_vehicleDynamics.SidewaysAcceleration));
            var sidewaysTiltAngle = Mathf.Lerp(0f, -Mathf.Sign(_vehicleDynamics.SidewaysAcceleration) * _visualConfig.MaxTiltAngleSideways, sidewaysLerpValue);
            eulerAngles.z = VehicleMath.AngleNormalize(eulerAngles.z, -180f); // convert to range -180..180, by default it is 0..360
            eulerAngles.z = Mathf.Lerp(eulerAngles.z, sidewaysTiltAngle, deltaTime * 5f);

            var forwardLerpValue = Mathf.InverseLerp(0f, _visualConfig.MaxAccelerationForward, Math.Abs(_vehicleDynamics.ForwardAcceleration));
            var forwardAngle = _vehicleDynamics.ForwardAcceleration < 0f ? _visualConfig.MaxTiltAngleForward : -_visualConfig.MaxTiltAngleBackward;
            var forwardTiltAngle = Mathf.Lerp(0f, forwardAngle, forwardLerpValue);
            eulerAngles.x = VehicleMath.AngleNormalize(eulerAngles.x, -180f);
            eulerAngles.x = Mathf.Lerp(eulerAngles.x, forwardTiltAngle, deltaTime * 5f);
            
            _body.localEulerAngles = eulerAngles;
        }

        private void UpdateWheelsVisuals()
        {
            foreach (var wheel in _wheels)
            {
                wheel.WheelCollider.UpdateTransform(wheel.WheelTransform);
                
                var sidewaysSpeed = _vehicleDynamics.GetSidewaysSpeedAtPosition(wheel.WheelTransform.position);
                var isStartGoingSkid = _vehicleDynamics.AbsForwardSpeed < _visualConfig.SkidMaxSpeedForward && _vehicleMovement.Acceleration > 0.5f && wheel.IsInvolvedInAcceleration;
                var isSidewaysSkid = sidewaysSpeed > _visualConfig.SkidMinSpeedSideways;
                var isHandbrakeSkid = _vehicleMovement.IsHandBraking && wheel.IsInvolvedInHandBraking;
                wheel.SkidEffect.IsEnabled = (isStartGoingSkid || isSidewaysSkid || isHandbrakeSkid) && wheel.WheelCollider.isGrounded;
            }
        }
    }
}