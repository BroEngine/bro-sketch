using Bro.Client.Context;
using UnityEngine;
using Game.Client.Vehicle;

namespace Game.Client.Battle
{
    public class VehicleInputProvider : IVehicleInputProvider
    {
        private const string AccelerationAxis = "acceleration";
        private const string BrakingAxis = "braking";
        private const string SteeringAxis = "steering";
        private const string HandbrakeAxis = "handbrake";
        private const float SpeedThresholdToSwitchDirection = 5f;
        
        private readonly VehicleBehaviour _vehicleBehaviour;
        private readonly VehicleDynamics _vehicleDynamics;
        private readonly VehicleConfiguration _vehicleConfiguration;
        private readonly Rigidbody _rigidbody;
        private readonly IClientContext _context;

        private VehicleInput _input = new VehicleInput();
        private bool _isMovingForward = true;
        
        VehicleInput IVehicleInputProvider.VehicleInput => _input;
        
        public VehicleInputProvider(VehicleBehaviour vehicleBehaviour, IClientContext context)
        {
            _context = context;
            _vehicleBehaviour = vehicleBehaviour;
            _vehicleDynamics = _vehicleBehaviour.Dynamics;
            _vehicleConfiguration = _vehicleBehaviour.Configuration;
            _rigidbody = _vehicleBehaviour.Rigidbody;
        }
        
        public void UpdateInput(float dt)
        {
            Keyboard(dt);
        }
        
        private void Keyboard(float dt)
        {
            var forwardAxis = UnityEngine.Input.GetAxis(AccelerationAxis);
            var backwardAxis = UnityEngine.Input.GetAxis(BrakingAxis);

            bool isBraking = backwardAxis > float.Epsilon;
            bool isAccelerating = forwardAxis > float.Epsilon;

            if (_isMovingForward && !isAccelerating && isBraking && _vehicleDynamics.ForwardSpeed < SpeedThresholdToSwitchDirection)
            {
                _isMovingForward = false;
            }

            if (!_isMovingForward && !isBraking && isAccelerating && _vehicleDynamics.ForwardSpeed > -SpeedThresholdToSwitchDirection && forwardAxis > float.Epsilon)
            {
                _isMovingForward = true;
            }
            
            if (_isMovingForward)
            {
                _input.Acceleration = forwardAxis;
                _input.Braking = backwardAxis;
                if (_vehicleDynamics.ForwardSpeed < 0f && _input.Acceleration > 0f)
                {
                    _input.Braking = Mathf.Max(_input.Acceleration, _input.Braking);
                }
            }
            else
            {
                _input.Acceleration = backwardAxis;
                _input.Braking = forwardAxis;
                
                if (_vehicleDynamics.ForwardSpeed > 0f && _input.Acceleration > 0f)
                {
                    _input.Braking = Mathf.Max(_input.Acceleration, _input.Braking);
                }
            }

            if (forwardAxis.IsZero() && backwardAxis.IsZero())
            {
                _input.Braking = 0.7f;
            }

            _input.Steering = UnityEngine.Input.GetAxis(SteeringAxis);
            _input.HandBraking = UnityEngine.Input.GetAxis(HandbrakeAxis);
            _input.IsForwardMoving = _isMovingForward;
        }
    }
}