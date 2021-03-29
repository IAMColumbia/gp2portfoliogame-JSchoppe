using UnityEngine;
using BruteDrive.EngineCore;
using BruteDrive.Vehicles;
using BruteDrive.Designer.Unity.Input;
using BruteDrive.Utilities.Unity;

namespace BruteDrive.Designer.Unity.Vehicles
{
    /// <summary>
    /// Wraps a scene instance of the core class Vehicle.
    /// </summary>
    public sealed class VehicleInstance : MonoBehaviour, IEditorWrapper<Vehicle>
    {
        #region Inspector Fields
        [Header("Optional Components")]
        [Tooltip("Adds a controller for this vehicle.")]
        [SerializeField] private VehicleController controller = default;
        [Tooltip("Adds a renderer for this vehicle.")]
        [SerializeField] private new VehicleRenderer renderer = default;
        [Header("Vehicle Attributes")]
        [Tooltip("The maximum steer angle in degrees in either direction.")]
        [Range(0f, 90f)][SerializeField] private float maxSteerDegrees = 5f;
        [Tooltip("Controls how quickly the tires turn in degrees per second. This modulates how much a vehicles eases into turns.")]
        [SerializeField] private float steerRate = 1f;
        [Tooltip("The maximum speed the vehicle can reach going forwards.")]
        [SerializeField] private float forwardsMaxSpeed = 6f;
        [Tooltip("The maximum speed the vehicle can reach going backwards.")]
        [SerializeField] private float reverseMaxSpeed = 2f;
        [Tooltip("The acceleration when driving forwards.")]
        [SerializeField] private float forwardsAcceleration = 2f;
        [Tooltip("The acceleration when braking and driving backwards.")]
        [SerializeField] private float reverseAcceleration = 0.5f;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            // Ensure valid inspector fields.
            forwardsMaxSpeed = Mathf.Max(0f, forwardsMaxSpeed);
            reverseMaxSpeed = Mathf.Max(0f, reverseMaxSpeed);
            steerRate = Mathf.Max(0f, steerRate);
        }
        #endregion

        private Vehicle vehicle;

        public Vehicle Instance()
        {
            // If the vehicle has not been created yet, use
            // the designer specified implementation to
            // generate a new vehicle.
            if (vehicle == null)
            {
                vehicle = new Vehicle(UnityTickService.GetProvider(UnityLoopType.FixedUpdate))
                {
                    MaxSteerDegrees = maxSteerDegrees,
                    SteerDegreesPerSecond = steerRate,
                    ForwardsMaxSpeed = forwardsMaxSpeed,
                    ReverseMaxSpeed = reverseMaxSpeed,
                    ForwardsAcceleration = forwardsAcceleration,
                    ReverseAcceleration = reverseAcceleration,
                    // It is ok if these fields are null.
                    // The vehicle can run headless and controlless.
                    Controller = controller,
                    Renderer = renderer
                };
            }
            return vehicle;
        }
    }
}
