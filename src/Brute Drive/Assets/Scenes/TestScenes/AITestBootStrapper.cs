using UnityEngine;
using BruteDriveCore.AI.Managers;
using BruteDriveUnity.Designer.Vehicles;
using BruteDriveUnity.Designer.Cameras;

namespace BruteDrive.BootStrappers
{
    /// <summary>
    /// The startup logic for the AI test scene.
    /// </summary>
    public sealed class AITestBootStrapper : MonoBehaviour
    {
        [Tooltip("The player controlled vehicle.")]
        [SerializeField] private VehicleInstance player = default;
        [SerializeField] private new VehicleCameraInstance camera = default;

        [SerializeField] private CruiserManager aiSpawner = default;
        [SerializeField] private int amountOfAgentsToSpawn = 1;

        private void Start()
        {
            // Initialize the instances for this scene.
            player.Instance();
            camera.Instance();
            for (int i = 0; i < amountOfAgentsToSpawn; i++)
                aiSpawner.TrySpawnCruiser();
        }
    }
}
