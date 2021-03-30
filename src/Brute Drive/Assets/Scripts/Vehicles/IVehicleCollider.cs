using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BruteDrive.Vehicles
{
    /// <summary>
    /// Implements a vehicle collision system.
    /// </summary>
    public interface IVehicleCollider
    {
        bool MoveSweep(Vector2 offset, out Vehicle hit);
    }
}
