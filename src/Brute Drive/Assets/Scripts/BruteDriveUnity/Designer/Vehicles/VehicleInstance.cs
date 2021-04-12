using UnityEngine;
using BruteDriveCore.Vehicles;
using UnityLibrary.TickWrappers;
using BruteDriveUnity.Designer.Input;
using UnityLibrary.InstanceWrappers;

namespace BruteDriveUnity.Designer.Vehicles
{
    /// <summary>
    /// Wraps a scene instance of the core class Vehicle.
    /// </summary>
    public sealed class VehicleInstance : UnityEditorWrapper<Vehicle>
    {
        #region Inspector Fields
        [Header("Optional Components")]
        [Tooltip("Adds a controller for this vehicle.")]
        [SerializeField] private VehicleController controller = default;
        [Tooltip("Adds a renderer for this vehicle.")]
        [SerializeField] private new VehicleRenderer renderer = default;
        [Tooltip("Adds collision to this vehicle.")]
        [SerializeField] private new VehicleCollider collider = default;
        [Header("Vehicle Attributes - Friction")]
        [Tooltip("The ambient friction/drag no matter the context of the vehicle. In meters per second squared.")]
        [SerializeField] private float ambientFriction = 0f;
        [Tooltip("The friction that is added when at max steer. This causes turns to slow the vehicle more.")]
        [SerializeField] private float maxSteerFriction = 0f;
        [Header("Vehicle Attributes - Steering")]
        [Tooltip("Controls how quickly the tires turn in degrees per second. This modulates how much a vehicles eases into turns.")]
        [SerializeField] private float steerDegreesPerSecond = 1f;
        [Tooltip("The maximum steer angle in degrees in either direction.")]
        [Range(0f, 90f)][SerializeField] private float maxSteerDegrees = 5f;
        [Tooltip("Controls how quickly the vehicle steers relative to its current speed.")]
        [SerializeField] private float steerRadiusFactor = 1f;
        [Header("Vehicle Attributes - Acceleration")]
        [Tooltip("The acceleration when driving forwards.")]
        [SerializeField] private float forwardsAcceleration = 2f;
        [Tooltip("The acceleration when braking and driving backwards.")]
        [SerializeField] private float reverseAcceleration = 0.5f;
        [Tooltip("The maximum speed the vehicle can reach going forwards.")]
        [SerializeField] private float forwardsMaxSpeed = 6f;
        [Tooltip("The maximum speed the vehicle can reach going backwards.")]
        [SerializeField] private float reverseMaxSpeed = 2f;
        [Header("Vehicle Attributes - Damage")]
        [Tooltip("The initial health of the vehicle.")]
        [Range(0f, 1f)][SerializeField] private float health = 1f;
        [Tooltip("Controls the speed where impacts do no damage.")]
        [SerializeField] private float impactResistance = 1f;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            ambientFriction = Mathf.Max(0f, ambientFriction);
            maxSteerFriction = Mathf.Max(0f, maxSteerFriction);
            steerDegreesPerSecond = Mathf.Max(0f, steerDegreesPerSecond);
            steerRadiusFactor = Mathf.Max(float.Epsilon, steerRadiusFactor);
            forwardsAcceleration = Mathf.Max(0f, forwardsAcceleration);
            reverseAcceleration = Mathf.Max(0f, reverseAcceleration);
            forwardsMaxSpeed = Mathf.Max(0f, forwardsMaxSpeed);
            reverseMaxSpeed = Mathf.Max(0f, reverseMaxSpeed);
            impactResistance = Mathf.Max(0f, impactResistance);
        }
        #endregion
        #region Wrapper Accessor
        /// <summary>
        /// Initializes the vehicle instance given the inspector values.
        /// </summary>
        /// <returns>The new vehicle instance.</returns>
        public override Vehicle Initialize()
            => new Vehicle(UnityTickService.GetProvider(UnityLoopType.FixedUpdate))
            {
                AmbientFriction = ambientFriction,
                MaxSteerFriction = maxSteerFriction,
                SteerDegreesPerSecond = steerDegreesPerSecond,
                SteerRadiusFactor = steerRadiusFactor,
                MaxSteerAngle = maxSteerDegrees,
                ForwardsAcceleration = forwardsAcceleration,
                ReverseAcceleration = reverseAcceleration,
                ForwardsMaxSpeed = forwardsMaxSpeed,
                ReverseMaxSpeed = reverseMaxSpeed,
                Health = health,
                ImpactResistance = impactResistance,
                // It is ok if these fields are null.
                // The vehicle can run headless and controlless.
                Controller = controller,
                Renderer = renderer,
                Collider = collider
            };
        #endregion
    }
}
