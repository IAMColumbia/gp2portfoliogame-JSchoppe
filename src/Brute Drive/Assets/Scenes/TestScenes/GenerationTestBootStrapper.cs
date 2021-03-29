using BruteDrive.Designer.Unity.Vehicles;
using BruteDrive.StageGeneration;
using Google.Maps.Unity.Intersections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BruteDrive.BootStrappers
{
    /// <summary>
    /// The startup logic for the generation test scene.
    /// </summary>
    public sealed class GenerationTestBootStrapper : MonoBehaviour, IGeneratorListener
    {
        #region Inspector Fields
        [Tooltip("The test vehicle for the player to control.")]
        [SerializeField] private VehicleInstance playerVehicle = default;
        [Tooltip("The stage generator to test.")]
        [SerializeField] private MapsIterator generator = default;

        [SerializeField] private RouteGenerator routeGenerator = default;
        #endregion
        #region Bootstrap Initialization
        private void Start()
        {
            // Run the map generation.
            generator.TryGenerate(this);
        }
        public void OnFailed()
        {
            
        }
        public void OnLoaded()
        {
            // Invoke the initialization of the player vehicle.
            RoadLatticeNode[] path = routeGenerator.GenerateRoute();

            for (int i = 1; i < path.Length; i++)
            {
                Debug.DrawLine(new Vector3(
                    path[i - 1].Location.x,
                    1f,
                    path[i - 1].Location.y),
                    new Vector3(
                    path[i].Location.x,
                    1f,
                    path[i].Location.y), Color.red);
            }

            playerVehicle.Instance();
        }
        #endregion
    }
}
