using BruteDrive.AI.Managers;
using BruteDrive.Designer.Unity.Vehicles;
using BruteDrive.StageGeneration;
using BruteDrive.Vehicles;
using Google.Maps.Unity.Intersections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BruteDrive.BootStrappers
{
    /// <summary>
    /// The startup logic for the escape mission.
    /// </summary>
    public sealed class EscapeMissionBootStrapper : MonoBehaviour, IGeneratorListener
    {
        #region Inspector Fields
        [Tooltip("The player instanec to load.")]
        [SerializeField] private VehicleInstance player = default;
        [Tooltip("Script that iteratively generates a stage outwards.")]
        [SerializeField] private MapsIterator generator = default;
        [Tooltip("The route generation script.")]
        [SerializeField] private RouteGenerator routeGenerator = default;
        [Tooltip("The manager for the opponent AI cruisers.")]
        [SerializeField] private CruiserManager cruiserManager = default;

        #endregion

        // Start is called before the first frame update
        private void Start()
        {
            // Run the map generation.
            generator.TryGenerate(this);
        }

        public void OnFailed()
        {
            // Fuck.
        }

        public void OnLoaded()
        {
            // Invoke the initialization of the player vehicle.
            RoadLatticeNode[] path = routeGenerator.GenerateRoute();

            Vehicle vehicle = player.Instance();

            Vector2 direction = path[1].Location - path[0].Location;

            vehicle.Location = path[0].Location;
            vehicle.Angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        }
    }
}
