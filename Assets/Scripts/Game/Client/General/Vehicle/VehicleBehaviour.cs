using UnityEngine;

namespace Game.Client.Vehicle
{
    [RequireComponent(typeof(VehicleConfiguration))]
    [RequireComponent(typeof(VehicleDynamics))]

    public class VehicleBehaviour : MonoBehaviour
    {
        [SerializeField] private VehicleConfiguration _vehicleConfiguration;
        [SerializeField] private VehicleDynamics _vehicleDynamics;
        
        private VehicleVisual _visual;
        private VehicleMovement _movement;
        private IVehicleInputProvider _inputProvider;
        
        public Rigidbody Rigidbody => _vehicleConfiguration.Rigidbody;
        public VehicleDynamics Dynamics => _vehicleDynamics;
        public VehicleMovement Movement => _movement;
        public VehicleConfiguration Configuration => _vehicleConfiguration;
        
        public void SetInputProvider(IVehicleInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
        }
        
        public void Awake()
        {
            Setup();
        }
        
        public void Setup()
        {
            _vehicleDynamics.Setup(_vehicleConfiguration.Rigidbody);
            _movement = new VehicleMovement(_vehicleConfiguration, _vehicleDynamics);
            _visual = new VehicleVisual(_vehicleConfiguration, _vehicleDynamics, _movement);
        }

        public void Place(Vector3 position, Quaternion rotation)
        {
            var rigidBody = _vehicleConfiguration.Rigidbody;
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.position = position;
            rigidBody.rotation = rotation;
        }
        
        public void Update()
        {
            if (_inputProvider != null)
            {
                _inputProvider.UpdateInput(Time.deltaTime);
                var movementInput = _inputProvider.VehicleInput;
                _movement.SetInput(ref movementInput);
            }
            
            _visual.Simulate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _movement.Simulate(Time.fixedDeltaTime);
            _vehicleDynamics.Simulate(Time.fixedDeltaTime);
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_vehicleConfiguration == null)
            {
                _vehicleConfiguration = GetComponent<VehicleConfiguration>();
            }

            if (_vehicleDynamics == null)
            {
                _vehicleDynamics = GetComponent<VehicleDynamics>();
            }
        }
        #endif
    }
}