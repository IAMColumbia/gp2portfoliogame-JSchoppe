using UnityEngine;
using BruteDriveCore.Vehicles;

namespace BruteDriveUnity.Designer.Input
{
    /// <summary>
    /// Base class for all vehicle controllers in Unity.
    /// </summary>
    public abstract class VehicleController : MonoBehaviour, IVehicleController
    {
        #region Vehicle Controller Requirements
        /// <summary>
        /// Accessor for the gas pedal pressed amount between 0 and 1.
        /// </summary>
        public float GasPedalAmount { get; protected set; }
        /// <summary>
        /// Accessor for the brake pedal pressed amount between 0 and 1.
        /// </summary>
        public float BrakePedalAmount { get; protected set; }
        /// <summary>
        /// Accessor for the steering angle between -1 and 1.
        /// </summary>
        public float SteeringAngle { get; protected set; }
        #endregion
    }
}
