namespace BruteDrive.Vehicles
{
    /// <summary>
    /// Defines a controller for a vehicle.
    /// </summary>
    public interface IVehicleController
    {
        #region Required Properties
        /// <summary>
        /// The amount that the gas pedal is pressed down between 0 and 1.
        /// </summary>
        float GasPedalAmount { get; }
        /// <summary>
        /// The amount that the brake pedal is pressed down between 0 and 1.
        /// </summary>
        float BrakePedalAmount { get; }
        /// <summary>
        /// The current angle of the steering wheel between -1 and 1.
        /// </summary>
        float SteeringAngle { get; }
        #endregion
    }
}
