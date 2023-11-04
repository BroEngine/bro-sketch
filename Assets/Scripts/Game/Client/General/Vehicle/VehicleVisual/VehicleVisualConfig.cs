using System;
using UnityEngine;

namespace Game.Client.Vehicle
{
    [Serializable]
    public class VehicleVisualConfig
    {
        [Range(0f,15f)]public float MaxTiltAngleForward = 6f;
        [Range(0f,15f)]public float MaxTiltAngleBackward = 12f;
        [Range(0f,60f)]public float MaxAccelerationForward = 30f;
        [Range(0f,15f)]public float MaxTiltAngleSideways = 8f;
        [Range(0f,30f)]public float MaxAccelerationSideways = 15f;

        [Range(0f, 30f)] public float SkidMaxSpeedForward = 15f;
        [Range(0f, 15f)] public float SkidMinSpeedSideways = 5f;
    }
}