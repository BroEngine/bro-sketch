using UnityEngine;

namespace Game.Client.Vehicle
{
    public class VehicleDynamics : MonoBehaviour
    {
        protected Rigidbody _rigidbody;

        private const float _speedThreshold = 0.05f;
        
        protected float _previousForwardSpeed;
        protected  float _previousSidewaysSpeed;

        public Vector3 InstantGlobalVelocity { get; private set; }
        public Vector3 GlobalVelocity { get; private set; }
        
        public Vector2 GlobalVelocity2d { get; private set; }
        
        public float Speed2d { get; private set; }
        public float ForwardSpeed { get; private set; }
        public float AbsForwardSpeed { get; private set; }
        public float SidewaysSpeed { get; private set; } // right - positive, left - negative value
        public float AbsSidewaysSpeed { get; private set; }
        
        public float AngularSpeed { get; protected set; }
        public float AbsAngularSpeed { get; protected set; }
        
        public Quaternion Rotation => _rigidbody.transform.rotation;
        public Vector3 Position => _rigidbody.transform.position;
        public Vector3 HorizontalForwardDirection => SetY(_rigidbody.transform.forward, 0f).normalized;
        public Vector3 HorizontalRightDirection => SetY(_rigidbody.transform.right, 0f).normalized;
        
        public bool IsMoving => !Speed2d.IsZero();
        
        public void Setup(Rigidbody rigidBody)
        {   
            _rigidbody = rigidBody;
        }
        
        private const float _accelerationThreshold = 0.1f;
        
        public float ForwardAcceleration { get; private set; }
        public float SidewaysAcceleration { get; private set; }
        
        public void Simulate(float deltaTime)
        {
            var instantForwardAcceleration = (ForwardSpeed - _previousForwardSpeed) / deltaTime;
            var instantSidewaysAcceleration = (SidewaysSpeed - _previousSidewaysSpeed) / deltaTime;
            _previousForwardSpeed = ForwardSpeed;
            _previousSidewaysSpeed = SidewaysSpeed;

            ForwardAcceleration = Mathf.Lerp(ForwardAcceleration, instantForwardAcceleration, deltaTime * 20f).Threshold(_accelerationThreshold);
            SidewaysAcceleration = Mathf.Lerp(SidewaysAcceleration, instantSidewaysAcceleration, deltaTime * 20f).Threshold(_accelerationThreshold);

            
            var horizontalForwardDirection = HorizontalForwardDirection;

            InstantGlobalVelocity = _rigidbody.velocity;
            GlobalVelocity = Vector3.Lerp(GlobalVelocity, InstantGlobalVelocity, deltaTime * 25f);

            GlobalVelocity2d = GetXZ(GlobalVelocity);
            Speed2d = GlobalVelocity2d.magnitude.Threshold(_speedThreshold);

            var forwardVelocity = Vector3.Project(GlobalVelocity, horizontalForwardDirection);
            AbsForwardSpeed = forwardVelocity.magnitude.Threshold(_speedThreshold);
            var isMovingForward = Vector3.Dot(GlobalVelocity, horizontalForwardDirection) > 0f;
            ForwardSpeed = AbsForwardSpeed * (isMovingForward ? 1f : -1f);

            var sideVelocity = (GlobalVelocity - forwardVelocity);
            sideVelocity.y = 0f;
            AbsSidewaysSpeed = sideVelocity.magnitude.Threshold(_speedThreshold);
            var isMovingRight = Vector3.Dot(GlobalVelocity, HorizontalRightDirection) > 0f;
            SidewaysSpeed = AbsSidewaysSpeed * (isMovingRight ? 1f : -1f);
            
            var angularSpeed = _rigidbody.angularVelocity;
            AngularSpeed = angularSpeed.y;
            AbsAngularSpeed = Mathf.Abs(angularSpeed.y);
        }
        
        public float GetSidewaysSpeedAtPosition(Vector3 worldPosition)
        {
            var horizontalForwardDirection = HorizontalForwardDirection;
            var globalVelocity = _rigidbody.GetPointVelocity(worldPosition);
            var forwardVelocity = Vector3.Project(globalVelocity, horizontalForwardDirection);
            var sideVelocity = globalVelocity - forwardVelocity;
            sideVelocity.y = 0;
            return sideVelocity.magnitude;
        }
        
        private static Vector2 GetXZ(Vector3 current)
        {
            return new Vector2(current.x, current.z);
        }
        
        private static Vector3 SetY(Vector3 current, float y)
        {
            return new Vector3(current.x, y, current.z);
        }
    }
}