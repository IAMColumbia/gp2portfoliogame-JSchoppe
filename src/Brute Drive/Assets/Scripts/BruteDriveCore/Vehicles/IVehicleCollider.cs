using GameLibrary.Math;

namespace BruteDriveCore.Vehicles
{
    /// <summary>
    /// Implements a vehicle collision system.
    /// </summary>
    public interface IVehicleCollider
    {
        bool MoveSweep(Vector2 offset, out Vehicle hit);
    }
}
