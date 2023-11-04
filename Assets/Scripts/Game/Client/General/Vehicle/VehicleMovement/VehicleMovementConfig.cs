using System;
using UnityEngine;

namespace Game.Client.Vehicle
{
    [Serializable]
    public class VehicleMovementConfig
    {
        [Serializable]
        public class WheelFrictionConfig
        {
            public float ExtremumSlip;
            public float ExtremumValue;
            public float AsymptoteSlip;
            public float AsymptoteValue;
            public float Stiffness;
        }

        [Serializable]
        public class HandbrakeUsageConfig
        {
            public float SlipFactorIncrease = 2f;
            public float SlipFactorDecrease = 2f;
            public float StiffnessFactorIncrease = 0.3f;
            public float StiffnessFactorDecrease = 2f;

            public float ExtremumSlipDuringBraking = 1f;
            public float StiffnessDuringBraking = 0.1f;
        }

        [Serializable]
        public class WheelMovementConfig
        {
            [Tooltip("1f - if wheel steering, 0f - no, -1f - counter steering(for example for rear wheels)")]
            public float SteeringFactor;
            public float MotorFactor;
            public float BrakeFactor;
            public float HandBrakeFactor;

            public WheelFrictionConfig ForwardFriction;
            public WheelFrictionConfig SidewaysFriction;
        }

        [Serializable]
        public class TrackedSteeringConfig
        {
            [Tooltip("Enable steering with tracks like tank")]
            public bool IsEnabled = false; 
            [Tooltip("The force to rotate, should be proportional to the mass to reach same speed")]
            public float TrackedSteeringForce = 15000f;
            [Tooltip("Steering only when angular speed less than current")]
            public float MaxAngularSpeed = 1.2f;
            [Tooltip("1f - no acceleration during full steering, 0f - to effect on acceleration")]
            [Range(0f,1f)]public float DecelerationRatio = 0.3f;
        }

        [Serializable]
        public class WheeledSteeringConfig
        {
            [Tooltip("Enable steering with tracks like car")]
            public bool IsEnabled = true; 
            [Range(0, 50f)] public float MaxSteeringAngle = 35f;
            [Range(0.1f, 2f)] public float SteeringSpeed = 0.3f;
        }

        // wheel collider
        // Suspension Spring
        // Spring Rate = vehicle mass / number of wheels * 2 * 9.81 / suspension distance
        // Damper rate = spring rate / 20

        public float MaxSpeedForward = 50f;
        public float MaxSpeedBackward = 10f;
        public float AccelerationForceForward = 1000f;
        public float AccelerationForceBackward = 500f;
        public float BrakeForce = 1000f;
        public float HandBrakeForce = 300f;
        public float BodyMassOffset = 0;
        public float BodyMass = 1200f;
        
        [Tooltip("when the vehicle stop accelerations if max speed exceeds")]
        public float MaxSpeedAccelerationStopThreshold = 1.0f;
        [Tooltip("when the vehicle starts use breaks if max speed exceeds")]
        public float MaxSpeedBreakingStartThreshold = 1.05f;
        
        
        public float NitroMaxSpeedFactor = 1.5f;
        public float NitroAccelerationForceFactor = 2f;

        public WheeledSteeringConfig WheeledSteering = new WheeledSteeringConfig();
        public TrackedSteeringConfig TrackedSteering = new TrackedSteeringConfig();

        public HandbrakeUsageConfig HandbrakeUsage = new HandbrakeUsageConfig();

        public WheelMovementConfig FrontWheelsMovement = new WheelMovementConfig()
        {
            SteeringFactor = 1f,
            MotorFactor = 0f,
            BrakeFactor = 0.7f,
            HandBrakeFactor = 0f,
            ForwardFriction = new WheelFrictionConfig()
            {
                ExtremumSlip = 0.4f,
                ExtremumValue = 1f,
                AsymptoteSlip = 0.8f,
                AsymptoteValue = 0.5f,
                Stiffness = 5
            },
            SidewaysFriction = new WheelFrictionConfig()
            {
                ExtremumSlip = 0.2f,
                ExtremumValue = 1f,
                AsymptoteSlip = 0.5f,
                AsymptoteValue = 0.75f,
                Stiffness = 5
            }
        };

        public WheelMovementConfig RearWheelsMovement = new WheelMovementConfig()
        {
            SteeringFactor = 0f,
            MotorFactor = 1f,
            BrakeFactor = 0.3f,
            HandBrakeFactor = 1f,
            ForwardFriction = new WheelFrictionConfig()
            {
                ExtremumSlip = 0.4f,
                ExtremumValue = 1f,
                AsymptoteSlip = 0.8f,
                AsymptoteValue = 0.5f,
                Stiffness = 5
            },
            SidewaysFriction = new WheelFrictionConfig()
            {
                ExtremumSlip = 0.2f,
                ExtremumValue = 1f,
                AsymptoteSlip = 0.5f,
                AsymptoteValue = 0.75f,
                Stiffness = 3
            }
        };

        public float GetAccelerationForce(bool isForwardMoving, bool isNitroActive)
        {
            if (isForwardMoving)
            {
                return isNitroActive ? AccelerationForceForward * NitroAccelerationForceFactor : AccelerationForceForward;
            }

            return AccelerationForceBackward;
        }
        

        public float GetMaxSpeed(bool isForwardMoving, bool isNitroActive)
        {
            if (isForwardMoving)
            {
                return isNitroActive ? MaxSpeedForward * NitroMaxSpeedFactor : MaxSpeedForward;
            }

            return MaxSpeedBackward;
        }
    }
}