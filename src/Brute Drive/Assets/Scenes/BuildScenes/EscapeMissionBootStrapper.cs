using BruteDriveCore.AI.Managers;
using BruteDriveCore.Vehicles;
using UnityEngine;
using Google.Maps.Unity.Intersections;
using BruteDriveUnity.Designer.Cameras;
using BruteDriveUnity.Designer.Vehicles;
using BruteDriveUnity.StageGeneration;
using UnityLibrary.Input;

namespace BruteDrive.BootStrappers
{
    /// <summary>
    /// The startup logic for the escape mission.
    /// </summary>
    public sealed class EscapeMissionBootStrapper : MonoBehaviour, IGeneratorListener
    {
        #region Inspector Fields
        [Tooltip("The player instance to load.")]
        [SerializeField] private VehicleInstance player = default;
        [Tooltip("The camera tied to the player.")]
        [SerializeField] private new VehicleCameraInstance camera = default;
        [Tooltip("Script that iteratively generates a stage outwards.")]
        [SerializeField] private MapsIterator generator = default;
        [Tooltip("The route generation script.")]
        [SerializeField] private RouteGenerator routeGenerator = default;
        [Tooltip("The manager for the opponent AI cruisers.")]
        [SerializeField] private CruiserManager cruiserManager = default;
        [Tooltip("Handler for touch input controls.")]
        [SerializeField] private TouchInputManager inputManager = default;
        #endregion

        private void Start()
        {
            // Run the map generation.
            generator.TryGenerate(this);
        }

        public void OnFailed()
        {
            // TODO this needs to be handled in a meaningful way.
        }

        public void OnLoaded()
        {
            inputManager.Initialize();

            // Invoke the initialization of the player vehicle.
            RoadLatticeNode[] path = routeGenerator.GenerateRoute();

            Vehicle vehicle = player.Instance();

            Vector2 direction = path[1].Location - path[0].Location;

            vehicle.Location = path[0].Location;
            vehicle.Angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

            camera.Instance();
        }
    }
}
