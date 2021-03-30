using BruteDrive.Utilities.Unity.Extensions;
using BruteDrive.Vehicles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BruteDrive.Designer.Unity.Vehicles
{
    /// <summary>
    /// Implements a vehicle collider in Unity.
    /// </summary>
    public sealed class VehicleCollider : MonoBehaviour, IVehicleCollider
    {
        [SerializeField] private VehicleInstance vehicle = default;
        [SerializeField] private BoxCollider2D hitBox = default;

        public Vehicle Vehicle { get; private set; }

        public bool MoveSweep(Vector2 offset, out Vehicle hit)
        {
            RaycastHit2D[] hitResults = new RaycastHit2D[1];

            int hits = hitBox.Cast(offset.normalized, hitResults, offset.magnitude);

            // Align the collider to the current vehicle position.
            hitBox.transform.position = Vehicle.Location;
            hitBox.transform.eulerAngles = Vector3.back * Vehicle.Angle;

            if (hits == 0)
            {
                Vehicle.Location += offset;
                hit = null;
                return false;
            }
            else
            {
                Vehicle.Location += offset * hitResults[0].fraction;
                VehicleCollider otherVehicle = hitResults[0].collider.GetComponent<VehicleCollider>();

                if (otherVehicle != null)
                    hit = otherVehicle.Vehicle;
                else
                    hit = null;
                return hit != null;
            }
        }

        private void Awake()
        {
            Vehicle = vehicle.Instance();
        }
    }
}

