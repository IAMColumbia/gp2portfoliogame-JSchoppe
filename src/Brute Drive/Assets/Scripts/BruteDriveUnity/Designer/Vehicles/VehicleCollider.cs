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

        public bool MoveSweep(GameLibrary.Math.Vector2 offset, out Vehicle hit)
        {
            // Align the collider to the current vehicle position.
            hitBox.transform.eulerAngles = Vector3.back * Vehicle.Angle;

            RaycastHit2D[] hitResults = new RaycastHit2D[1];
            int hits = hitBox.Cast(offset.GetNormalized(), hitResults, offset.GetLength());


            if (hits == 0)
            {
                Vehicle.Location += (Vector2)offset;
                hitBox.transform.position = Vehicle.Location;
                hit = null;
                return false;
            }
            else
            {
                Vehicle.Location = Vehicle.Location + (Vector2)(offset * (hitResults[0].fraction) - offset.GetNormalized() * 0.005f);

                Vector2 remaining = offset * (1f - hitResults[0].fraction);
                Vector2 alongWall = new Vector2(hitResults[0].normal.y, hitResults[0].normal.x);
                Vehicle.Location += alongWall * (Vector2.Dot(remaining, alongWall) / Vector2.Dot(alongWall, alongWall));

                hitBox.transform.position = Vehicle.Location;
                //Vehicle.Speed = 0f;
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

