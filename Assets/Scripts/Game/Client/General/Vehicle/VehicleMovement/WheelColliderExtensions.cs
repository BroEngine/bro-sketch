using UnityEngine;

namespace Game.Client.Vehicle
{
    public static class WheelColliderExtensions
    {
        public static void SetSidewaysStiffness(this WheelCollider collider, float value)
        {
            var sidewaysFriction =  collider.sidewaysFriction;
            sidewaysFriction.stiffness = value;
            collider.sidewaysFriction = sidewaysFriction;
        }
        
        public static void SetSidewaysExtremumSlip(this WheelCollider collider, float value)
        {
            var sidewaysFriction =  collider.sidewaysFriction;
            sidewaysFriction.extremumSlip = value;
            collider.sidewaysFriction = sidewaysFriction;
        }

        public static void UpdateTransform(this WheelCollider collider, Transform targetTransform)
        {
            collider.GetWorldPose(out var position, out var rotation);
            targetTransform.SetPositionAndRotation(position, rotation);
        }

        public static float GetLinearRotationSpeed(this WheelCollider collider)
        {
            return (2f * Mathf.PI * collider.radius * collider.rpm) / 60f;
        }
    }
}