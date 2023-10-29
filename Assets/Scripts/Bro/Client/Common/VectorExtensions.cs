using UnityEngine;

namespace Bro.Client
{
    public static class VectorExtensions
    {
        public static Vector3 ToXOZ(this Vector3 vector3)
        {
            return new Vector3(vector3.x, 0, vector3.z);
        }
    }
}