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

        private const int ROTATION_SWEEPS = 4;
        private const int TRANSLATION_SWEEPS = 6;
        private const float SWEEP_EPSILON = 0.001f;

        public Vehicle Vehicle { get; private set; }


        public VehicleSweepResult MoveSweep(float rotation, float distance)
        {
            // Initialize output structure.
            List<VehicleHitResult> vehicleHitResults =
                new List<VehicleHitResult>();
            // Apply the rotation; see if it causes any intersections.
            float amountRotated = 0f;
            float startingRotation = hitBox.transform.eulerAngles.z;
            hitBox.transform.eulerAngles += Vector3.back * rotation;
            if (hitBox.OverlapCollider(default, new Collider2D[1]) > 0)
            {
                // Use bisection to determine the least amount
                // of rotation we have to undo to clear the overlap.
                float bisectionFactor = 1f;
                float bisectionAccumulator = 0f;
                bool isIntersecting = true;
                for (int i = 0; i < ROTATION_SWEEPS; i++)
                {
                    bisectionFactor *= 0.5f;
                    // Apply the bisection step.
                    if (isIntersecting)
                    {
                        bisectionAccumulator += bisectionFactor;
                        hitBox.transform.eulerAngles +=
                            Vector3.forward * rotation * bisectionFactor;
                    }
                    else
                    {
                        bisectionAccumulator -= bisectionFactor;
                        hitBox.transform.eulerAngles -=
                            Vector3.forward * rotation * bisectionFactor;
                    }
                    // Recheck collision.
                    isIntersecting = hitBox.OverlapCollider(
                        default, new Collider2D[1]) > 0;
                    // Mark this as a new best amountRotated.
                    if (!isIntersecting)
                        amountRotated = rotation - bisectionAccumulator;
                }
                // Apply the final best rotation that does not intersect.
                hitBox.transform.eulerAngles = Vector3.forward * (startingRotation + amountRotated);
            }
            else
                amountRotated = rotation;

            // Translate the collider, brushing up against edges iteratively.
            // A limit is placed on how many times this can iterate to avoid infinite loops.
            RaycastHit2D[] hitResults = new RaycastHit2D[1];
            Vector2 direction = hitBox.transform.up;
            int sweepIterations = 0;
            float startingDistance = distance;
            while (distance != 0f && sweepIterations < TRANSLATION_SWEEPS)
            {
                sweepIterations++;
                // Cast the box collider against the scene.
                int hits = hitBox.Cast(direction, hitResults, distance);
                // If there are no hits, we can move directly forward.
                if (hits == 0)
                {
                    Vehicle.Location = (Vector2)Vehicle.Location + direction * distance;
                    hitBox.transform.position = (Vector2)Vehicle.Location;
                    distance = 0f;
                }
                // Otherwise there is a hit.
                else
                {
                    // Get the translation to reach this hit.
                    // A slight offset is added to ensure the colliders
                    // do not intersect.
                    Vector2 translationToHit =
                        direction * distance * (hitResults[0].fraction)
                        - direction * SWEEP_EPSILON;
                    // Move the collider to the edge.
                    Vehicle.Location = (Vector2)Vehicle.Location + translationToHit;
                    hitBox.transform.position = (Vector2)Vehicle.Location;

                    distance -= translationToHit.magnitude;

                    // Get the vector passing through the wall after the intersection.
                    Vector2 remaining = (direction * distance) - translationToHit;
                    // Get the vector parallel to the edge that was hit.
                    // Swapping x and y values yields this, and we do not
                    // care about the sign (handled by dot product).
                    Vector2 alongWall = new Vector2(
                        hitResults[0].normal.y,
                        hitResults[0].normal.x);
                    // Project the remaining vector along the edge.
                    // TODO Vector2.Project should be abstracted.
                    direction = alongWall *
                        (Vector2.Dot(remaining, alongWall) / Vector2.Dot(alongWall, alongWall));

                    // Check to see if we hit a vehicle collider.
                    VehicleCollider otherVehicle =
                        hitResults[0].collider.GetComponent<VehicleCollider>();
                    // Save this vehicle hit to notify it later.
                    if (otherVehicle != null)
                    {
                        // 0f is a test; should pass more descriptive data.
                        vehicleHitResults.Add(new VehicleHitResult(
                             otherVehicle.Vehicle,
                             0f));
                    }
                }
            }
            // Return details about what happened in the sweep.
            return new VehicleSweepResult(
                amountRotated,
                startingDistance - distance,
                vehicleHitResults.ToArray());
        }

        private void Awake()
        {
            Vehicle = vehicle.Instance();
        }
    }
}

