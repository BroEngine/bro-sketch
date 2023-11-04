namespace Game.Client.Vehicle
{
    public interface IVehicleInputProvider
    {
        VehicleInput VehicleInput { get; }
        void UpdateInput(float dt);
    }
}