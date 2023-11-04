using UnityEngine;

namespace Game.Client
{
    public static class LayersConfig
    {
        public static bool IsVehicle(this GameObject gameObject)
        {
            return gameObject.layer == Vehicle.Id;
        }
        
        public static bool IsObstacle(this GameObject gameObject)
        {
            return gameObject.layer == Obstacle.Id;
        }
        
        public static class Obstacle
        {
            private const string Name = "Obstacle";
            public static readonly int Id =  UnityEngine.LayerMask.NameToLayer(Name);
        }
         
        public static class Vehicle
        {
            private const string Name = "Vehicle";
            public static readonly int Id =  UnityEngine.LayerMask.NameToLayer(Name);
        }
    }
}