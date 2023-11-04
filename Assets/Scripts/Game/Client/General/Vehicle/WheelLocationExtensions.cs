namespace Game.Client.Vehicle
{
    public static class WheelLocationExtensions
    {
        public static bool IsFront(this VehicleConfiguration.WheelLocation me)
        {
            return me == VehicleConfiguration.WheelLocation.FrontLeft || me == VehicleConfiguration.WheelLocation.FrontRight;
        }
        
        public static bool IsLeft(this VehicleConfiguration.WheelLocation me)
        {
            return me == VehicleConfiguration.WheelLocation.FrontLeft || me == VehicleConfiguration.WheelLocation.RearLeft;
        }
        
        public static bool IsRight(this VehicleConfiguration.WheelLocation me)
        {
            return me == VehicleConfiguration.WheelLocation.FrontRight || me == VehicleConfiguration.WheelLocation.RearRight;
        }
        
        public static bool IsRear(this VehicleConfiguration.WheelLocation me)
        {
            return me == VehicleConfiguration.WheelLocation.RearLeft || me == VehicleConfiguration.WheelLocation.RearRight;
        }
    }
}