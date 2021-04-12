using UnityEngine;
using BruteDriveUnity.Designer.Vehicles;

namespace BruteDrive.BootStrappers
{
    /// <summary>
    /// The startup logic for the AI test scene.
    /// </summary>
    public sealed class AITestBootStrapper : MonoBehaviour
    {
        [Tooltip("The player controlled vehicle.")]
        [SerializeField] private VehicleInstance player = default;


        private void Start()
        {
            // Initialize the player instance.
            player.Instance();
        }
    }
}
