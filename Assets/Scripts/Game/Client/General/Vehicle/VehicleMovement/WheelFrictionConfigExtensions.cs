using UnityEngine;

namespace Game.Client.Vehicle
{
    public static class WheelFrictionConfigExtensions
    {
        public static WheelFrictionCurve ToWheelFrictionCurve(this VehicleMovementConfig.WheelFrictionConfig cfg)
        {
            return new WheelFrictionCurve
            {
                stiffness = cfg.Stiffness,
                asymptoteSlip = cfg.AsymptoteSlip,
                asymptoteValue = cfg.AsymptoteValue,
                extremumSlip = cfg.ExtremumSlip,
                extremumValue = cfg.ExtremumValue
            };
        }
    }
}