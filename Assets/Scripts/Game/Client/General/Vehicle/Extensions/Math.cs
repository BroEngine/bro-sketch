using System;

namespace Game.Client.Vehicle
{
    public static class VehicleMath
    {
        public static float AngleNormalize(float angle, float minValue = -180f)
        {
            float maxValue = minValue + 360f;
            if (angle >= minValue && angle < maxValue)
            {
                return angle;
            }

            if (angle < minValue)
            {
                angle += 360f * (float) ((int) (minValue - angle) / 360 + 1);
                return angle;
            }

            angle -= 360f * (float) ((int) (angle - maxValue) / 360 + 1);
            return angle;
        }

        public static float AngleDelta(float fromAngle, float toAngle)
        {
            var result = AngleNormalize(toAngle) - AngleNormalize(fromAngle);
            if (result < -180f)
            {
                result += 360f;
            }
            else if(result > 180f)
            {
                result -= 360f;
            }
            return result;
        }
        
        public static float Threshold(this float target, float value)
        {
            return Math.Abs(target) <= value ? 0f : target;
        }

        public static float Validate(this float target)
        {
            if (float.IsNaN(target))
            {
                target = 0;
            }
            
            return target;
        }

        public static bool IsZero(this float target)
        {
            return Math.Abs(target) <= float.Epsilon;
        }
        
        public static float Clamp01(this float target)
        {
            if (target < 0f)
            {
                return 0f;
            }

            if (target > 1f)
            {
                return 1f;
            }

            return target;
        }
    }
}