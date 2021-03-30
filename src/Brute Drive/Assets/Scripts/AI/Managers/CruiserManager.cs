using BruteDrive.AI.Actors;
using BruteDrive.Designer.Unity.Vehicles;
using BruteDrive.Utilities.Unity.Extensions;
using BruteDrive.Vehicles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BruteDrive.AI.Managers
{
    // TODO decouple from MonoBehaviour.
    /// <summary>
    /// Manages a collection of cruisers.
    /// </summary>
    public sealed class CruiserManager : MonoBehaviour
    {

        [SerializeField] private VehicleInstance cruiserTarget = default;

        [Tooltip("The locations that cruisers can spawn.")]
        [SerializeField] private Vector3[] spawnLocations = default;
        [Tooltip("The prefab used for the cruiser. Should contain a vehicle instance.")]
        [SerializeField] private GameObject cruiserPrefab = default;
        [Tooltip("The amount of active cruisers that will try to spawn.")]
        [SerializeField] private int targetCount = 1;
        [Tooltip("Prevents spawning a cruiser if another cruiser is within this distance of the location.")]
        [SerializeField] private float spawnRoom = 5f;

        public Vector2[] SpawnLocations
        {
            get => spawnLocations.TopDownFlatten();
            set => spawnLocations = value.TopDownUnflatten();
        }

        public float SpawnRoom
        {
            get => spawnRoom;
            set
            {
                value = Mathf.Max(0f, value);
                if (value != spawnRoom)
                    spawnRoom = value;
            }
        }

        private List<CruiserAgent> deployedCruisers;

        private void Awake()
        {
            deployedCruisers = new List<CruiserAgent>();
            TrySpawnCruiser();
        }

        private void TrySpawnCruiser()
        {
            float roomSquared = spawnRoom * spawnRoom;
            foreach (Vector3 location in spawnLocations)
            {
                bool isBlocked = false;
                foreach (CruiserAgent agent in deployedCruisers)
                {
                    if (Vector2.SqrMagnitude(agent.Vehicle.Location - location.TopDownFlatten()) < roomSquared)
                    {
                        isBlocked = true;
                        break;
                    }
                }
                if (!isBlocked)
                {
                    GameObject jd = Instantiate(cruiserPrefab);
                    deployedCruisers.Add(new CruiserAgent(jd.GetComponentInChildren<VehicleInstance>().Instance()));
                    deployedCruisers[deployedCruisers.Count - 1].Vehicle.Location = location.TopDownFlatten();
                    deployedCruisers[deployedCruisers.Count - 1].Target = cruiserTarget.Instance();

                    deployedCruisers[deployedCruisers.Count - 1].CurrentState = CruiserAgent.State.Charging;
                    break;
                }
            }
        }


        // Update is called once per frame
        private void FixedUpdate()
        {
            foreach (CruiserAgent agent in deployedCruisers)
                agent.Tick(Time.fixedDeltaTime);
        }
    }
}
