using UnityEngine;
using BruteDriveUnity.Designer.Vehicles;

namespace BruteDrive.BootStrappers
{
    /// <summary>
    /// Initiates the underlying core logic for the drive test scene.
    /// </summary>
    public sealed class DriveTestBootStrapper : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The test vehicle for the player to control.")]
        [SerializeField] private VehicleInstance playerVehicle = default;
        #endregion
        #region Bootstrap Initialization
        private void Start()
        {
            // Invoke the initialization of the player vehicle.
            playerVehicle.Instance();
        }
        #endregion
    }
}
