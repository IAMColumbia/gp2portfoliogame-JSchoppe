using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BruteDriveCore.Vehicles;
using UnityLibrary.Extensions;

namespace BruteDriveUnity.Designer.Vehicles
{
    /// <summary>
    /// Implements a vehicle renderer in Unity.
    /// </summary>
    public sealed class VehicleRenderer : MonoBehaviour, IVehicleRenderer
    {
        #region Inspector Fields
        [Tooltip("The transform that will be scaled along the x axis to reflect health.")]
        [SerializeField] private Transform healthMeterTransform = default;
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

        public float Health
        {
            set => healthMeterTransform.localScale = new Vector3(
                value,
                healthMeterTransform.localScale.y,
                healthMeterTransform.localScale.z);
        }

        public void OnDestroyed()
        {
            
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
