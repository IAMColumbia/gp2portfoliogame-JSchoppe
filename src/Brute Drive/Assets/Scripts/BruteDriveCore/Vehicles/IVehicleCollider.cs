namespace BruteDriveCore.Vehicles
{
    /// <summary>
    /// Implements a vehicle collision system.
    /// </summary>
    public interface IVehicleCollider
    {
        /// <summary>
        /// Implements a movement sweep with rotation and offset.
        /// </summary>
        /// <param name="rotation">The rotation of the vehicle in degrees.</param>
        /// <param name="distance">The distance to move after rotating.</param>
        /// <returns>Sweep results from the sweep.</returns>
        VehicleSweepResult MoveSweep(float rotation, float distance);
    }

    public sealed class VehicleSweepResult
    {
        public readonly float actualRotation;
        public readonly float actualDistance;
        public readonly VehicleHitResult[] vehicleHits;

        public VehicleSweepResult(
            float actualRotation,
            float actualDistance,
            VehicleHitResult[] vehicleHits)
        {
            this.actualRotation = actualRotation;
            this.actualDistance = actualDistance;
            this.vehicleHits = vehicleHits;
        }
    }

    public sealed class VehicleHitResult
    {
        public readonly Vehicle vehicle;
        public readonly float intersectionAmount;

        public VehicleHitResult(
            Vehicle vehicle,
            float intersectionAmount)
        {
            this.vehicle = vehicle;
            this.intersectionAmount = intersectionAmount;
        }
    }
}
