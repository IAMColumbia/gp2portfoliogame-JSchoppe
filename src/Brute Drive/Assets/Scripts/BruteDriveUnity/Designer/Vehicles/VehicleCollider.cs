using BruteDriveCore.Vehicles;
using BruteDriveUnity.Designer.Vehicles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BruteDriveUnity.Designer.Vehicles
{
    /// <summary>
    /// Implements a vehicle collider in Unity.
    /// </summary>
    public sealed class VehicleCollider : MonoBehaviour, IVehicleCollider
    {
        [SerializeField] private VehicleInstance vehicle = default;
        [SerializeField] private BoxCollider2D hitBox = default;

        public Vehicle Vehicle { get; private set; }

        private const int MAX_SWEEP_ITERATIONS = 10;
        private const float SWEEP_EPSILON = 0.001f;

        public bool MoveSweep(GameLibrary.Math.Vector2 offset, out Vehicle hit)
        {
            hit = null;
            // Align the collider to the current vehicle orientation.
            hitBox.transform.eulerAngles = Vector3.back * Vehicle.Angle;
            // Sweep the collider, brushing up against edges iteratively.
            // A limit is placed on how many times this can iterate to avoid infinite loops.
            RaycastHit2D[] hitResults = new RaycastHit2D[1];
            int sweepIterations = 0;
            while (offset != Vector2.zero && sweepIterations < MAX_SWEEP_ITERATIONS)
            {
                sweepIterations++;
                // Cast the box collider against the scene.
                int hits = hitBox.Cast(offset.GetNormalized(), hitResults, offset.GetLength());
                // If there are no hits, we can move directly forward.
                if (hits == 0)
                {
                    Vehicle.Location += offset;
                    hitBox.transform.position = (Vector2)Vehicle.Location;
                    offset = Vector2.zero;
                }
                // Otherwise there is a hit.
                else
                {
                    // Get the translation to reach this hit.
                    // A slight offset is added to ensure the colliders
                    // do not intersect.
                    Vector2 translationToHit =
                        offset * (hitResults[0].fraction)
                        - offset.GetNormalized() * SWEEP_EPSILON;
                    // Move the collider to the edge.
                    Vehicle.Location = (Vector2)Vehicle.Location + translationToHit;
                    hitBox.transform.position = (Vector2)Vehicle.Location;

                    // Get the vector passing through the wall after the intersection.
                    Vector2 remaining = (Vector2)offset - translationToHit;
                    // Get the vector parallel to the edge that was hit.
                    // Swapping x and y values yields this, and we do not
                    // care about the sign (handled by dot product).
                    Vector2 alongWall = new Vector2(
                        hitResults[0].normal.y,
                        hitResults[0].normal.x);
                    // Project the remaining vector along the edge.
                    // TODO Vector2.Project should be abstracted.
                    offset = alongWall *
                        (Vector2.Dot(remaining, alongWall) / Vector2.Dot(alongWall, alongWall));

                    // Check to see if we hit a vehicle collider.
                    VehicleCollider otherVehicle =
                        hitResults[0].collider.GetComponent<VehicleCollider>();
                    // Save this vehicle hit to notify it later.
                    if (otherVehicle != null)
                        hit = otherVehicle.Vehicle;
                }
            }
            // Return true if a vehicle was hit in this sweep.
            return hit != null;
        }

        private void Awake()
        {
            Vehicle = vehicle.Instance();
        }
    }
}

