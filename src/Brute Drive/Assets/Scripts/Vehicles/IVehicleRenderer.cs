using UnityEngine; // Only used for Vector2.

namespace BruteDrive.Vehicles
{
    /// <summary>
    /// Implements the rendered elements of the vehicle.
    /// </summary>
    public interface IVehicleRenderer
    {
        #region Property Requirements
        /// <summary>
        /// Updates the position of the rendered vehicle.
        /// </summary>
        Vector2 Position { set; }
        /// <summary>
        /// Updates the direction of the rendered vehicle.
        /// </summary>
        Vector2 Forwards { set; }
        /// <summary>
        /// Updates the steer angle of the rendered vehicle.
        /// </summary>
        float SteerAngle { set; }
        /// <summary>
        /// Updates the rendered health for this vehicle.
        /// </summary>
        float Health { set; }
        #endregion
        #region Method Requirements
        /// <summary>
        /// Called once when the vehicle has been destroyed.
        /// </summary>
        void OnDestroyed();
        #endregion
    }
}
