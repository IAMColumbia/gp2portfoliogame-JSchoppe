using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BruteDrive.Vehicles;
using BruteDrive.Utilities.Unity.Extensions;

namespace BruteDrive.Designer.Unity
{
    /// <summary>
    /// Implements a vehicle renderer in Unity.
    /// </summary>
    public sealed class VehicleRenderer : MonoBehaviour, IVehicleRenderer
    {
        #region Inspector Fields
        [Tooltip("All the wheels that will spin around their local x axes.")]
        [SerializeField] private Transform[] allWheels = default;
        [Tooltip("All the wheels on the vehicle that will rotate their local forwards direction.")]
        [SerializeField] private Transform[] turningWheels = default;
        #endregion

        public Vector2 Position
        {
            set
            {
                // Convert into 2D top down space from the given vector2.
                transform.position = new Vector3(value.x, 0f, value.y);
            }
        }

        public float SteerAngle
        {
            set
            {
                // Apply local rotation to all tires that turn.
                foreach (Transform wheel in turningWheels)
                    wheel.SetLocalEulerAngleY(value);
            }
        }

        public Vector2 Forwards
        {
            set
            {
                transform.forward = new Vector3(value.x, 0f, value.y);
            }
        }

        void Awake()
        {
#if DEBUG
            // Catch designer mistakes.
            if (allWheels == null)
                Debug.LogError("Vehicle Renderer must specify wheel transforms!", this);
            if (turningWheels == null)
                Debug.LogError("Vehicle Renderer must specify turning wheel transforms!", this);
#endif
        }
    }
}
