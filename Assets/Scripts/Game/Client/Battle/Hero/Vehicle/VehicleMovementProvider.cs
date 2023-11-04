using System;
using Bro.Client.Context;
using UnityEngine;
using Game.Client.Vehicle;

namespace Game.Client.Battle
{
    public class VehicleMovementProvider : IVehicleMovementInputProvider
    {
        private readonly VehicleBehaviour _vehicleBehaviour;
        private readonly VehicleDynamics _vehicleDynamics;
        private readonly VehicleConfiguration _vehicleConfiguration;
        private readonly Rigidbody _rigidbody;
        private readonly IClientContext _context;

        private VehicleMovementInput _input = new VehicleMovementInput();
        
        VehicleMovementInput IVehicleMovementInputProvider.VehicleMovementInput => _input;
        
        public VehicleMovementProvider(VehicleBehaviour vehicleBehaviour, IClientContext context)
        {
            _context = context;
            _vehicleBehaviour = vehicleBehaviour;
            _vehicleDynamics = _vehicleBehaviour.Dynamics;
            _vehicleConfiguration = _vehicleBehaviour.Configuration;
            _rigidbody = _vehicleBehaviour.Rigidbody;
        }
        
        public void UpdateInput(float dt)
        {
            // todo
        }
    }
}