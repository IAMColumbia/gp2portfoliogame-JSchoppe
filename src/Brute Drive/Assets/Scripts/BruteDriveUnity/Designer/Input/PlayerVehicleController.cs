using UnityEngine.InputSystem;

namespace BruteDriveUnity.Designer.Input
{
    /// <summary>
    /// Implements a vehicle controller using the new input system.
    /// </summary>
    public sealed class PlayerVehicleController : VehicleController
    {
        #region Input Listeners
        // Listens to the new input system to update
        // underlying control values.
        public void RecieveGasPedal(InputAction.CallbackContext context)
        {
            GasPedalAmount = context.ReadValue<float>();
        }
        public void RecieveBrakePedal(InputAction.CallbackContext context)
        {
            BrakePedalAmount = context.ReadValue<float>();
        }
        public void RecieveSteeringAngle(InputAction.CallbackContext context)
        {
            SteeringAngle = context.ReadValue<float>();
        }
        #endregion
    }
}
