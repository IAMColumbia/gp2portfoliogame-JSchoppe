using BruteDriveCore.AI.Managers;
using BruteDriveCore.Vehicles;
using UnityEngine;
using Google.Maps.Unity.Intersections;
using BruteDriveUnity.Designer.Cameras;
using BruteDriveUnity.Designer.Vehicles;
using BruteDriveUnity.StageGeneration;
using UnityLibrary.Input;
using BruteDriveCore.Objectives;
using UnityLibrary.TickWrappers;
using System.Collections.Generic;
using UnityLibrary.TopDown2D;
using BruteDriveUnity.Designer.Objectives;
using UnityLibrary.InstanceWrappers.Indicators;

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

        [SerializeField] private GameObject waypointPrefab = default;
        [SerializeField] private float minWaypointDistance = 10f;
        [SerializeField] private CompassInstance compass = default;

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

            // Generate waypoint rings along the path.
            RoadLatticeNode[] path = routeGenerator.GenerateRoute();

            List<IWaypoint> waypoints = new List<IWaypoint>();
            float distanceAccumulator = 0f;
            for (int i = 1; i < path.Length - 1; i++)
            {
                distanceAccumulator += Vector2.Distance(
                    path[i - 1].Location, path[i].Location);
                if (distanceAccumulator > minWaypointDistance)
                {
                    GameObject newWaypoint = Instantiate(
                        waypointPrefab,
                         path[i].Location.TopDownUnflatten(),
                         Quaternion.LookRotation(
                             path[i + 1].Location.TopDownUnflatten()
                             - path[i - 1].Location.TopDownUnflatten()));
                    distanceAccumulator = 0f;
                    waypoints.Add(newWaypoint.GetComponent<Waypoint>());
                }
            }
            GameObject lastWaypoint = Instantiate(
                        waypointPrefab,
                         path[path.Length - 1].Location.TopDownUnflatten(),
                         Quaternion.LookRotation(
                             path[path.Length - 1].Location.TopDownUnflatten()
                             - path[path.Length - 2].Location.TopDownUnflatten()));
            distanceAccumulator = 0f;
            waypoints.Add(lastWaypoint.GetComponent<Waypoint>());


            WaypointsObjective objective = new WaypointsObjective(
                UnityTickService.GetProvider(UnityLoopType.FixedUpdate),
                waypoints.ToArray());

            objective.WaypointChanged += Objective_WaypointChanged;


            Vehicle vehicle = player.Instance();

            Vector2 direction = path[1].Location - path[0].Location;

            vehicle.Location = path[0].Location;
            vehicle.Angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

            objective.TriggeringVehicles.Add(vehicle);

            objective.StartObjective();
            objective.Completed += Completed;

            camera.Instance();
        }

        private void Objective_WaypointChanged(IWaypoint newWaypoint)
        {
            compass.Instance().Target = ((Vector2)newWaypoint.Location).TopDownUnflatten();
        }

        private void Completed(Objective completedObjective)
        {
            // Do some stuff here.
        }
    }
}
