using BruteDriveCore.Cameras;
using BruteDriveUnity.Designer.Vehicles;
using UnityLibrary.TickWrappers;
using GameLibrary.Transforms;
using UnityEngine;
using UnityLibrary.InstanceWrappers;

namespace BruteDriveUnity.Designer.Cameras
{
    /// <summary>
    /// Wraps a scene instance of the core class Vehicle Camera.
    /// </summary>
    public sealed class VehicleCameraInstance : UnityEditorWrapper<VehicleCamera>, IOrientedTransform3D
    {
        #region Inspector Fields
        [Header("Target Reference")]
        [Tooltip("The vehicle that this camera will follow.")]
        [SerializeField] private VehicleInstance vehicleInstance = default;
        [Header("Camera Parameters")]
        [Tooltip("The elevation to point the camera at the vehicle location.")]
        [SerializeField] private float lookElevation = 6f;
        [Tooltip("The required speed before the camera will react.")]
        [SerializeField] private float speedThreshold = 0.5f;
        [Tooltip("The speed at which the camera will rotate when the vehicle is turning.")]
        [SerializeField] private float degreesPerSecond = 180f;
        [Tooltip("The ")]
        [SerializeField] private float unitsPerSecond = 1f;
        [Header("Camera Parameters - Min Speed")]
        [Tooltip("The distance from the vehicle at minimum speed zero.")]
        [SerializeField] private float minBoomDistance = 8f;
        [Tooltip("The height from the vehicle at minimum speed zero.")]
        [SerializeField] private float minBoomHeight = 6f;
        [Header("Camera Parameters - Max Speed")]
        [Tooltip("The distance from the vehicle at the maximum vehicle speed.")]
        [SerializeField] private float maxBoomDistance = 12f;
        [Tooltip("The height from the vehicle at the maximum vehicle speed.")]
        [SerializeField] private float maxBoomHeight = 8f;
        #endregion
#if DEBUG
        #region Inspector Validation
        private void OnValidate()
        {
            
        }
        private void Awake()
        {
            // Warn designer of bad data.
            if (vehicleInstance == null)
                Debug.LogError("Camera needs a vehicle to follow!", this);
        }
        #endregion
#endif
        #region Oriented Transform Implementation
        /// <summary>
        /// Linked to the transform global position.
        /// </summary>
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        /// <summary>
        /// Linked to the transform forwards direction.
        /// </summary>
        public Vector3 Forwards
        {
            get => transform.forward;
            set => transform.forward = value;
        }
        #endregion
        #region Wrapper Accessor
        /// <summary>
        /// Initialized the wrapped vehicle camera.
        /// </summary>
        /// <returns>The vehicle camera class held in this Unity instance.</returns>
        public override VehicleCamera Initialize() =>
            new VehicleCamera(this, UnityTickService.GetProvider(UnityLoopType.FixedUpdate))
            {
                Vehicle = vehicleInstance.Instance(),
                AnimationDegreesPerSecond = degreesPerSecond,
                AnimationUnitsPerSecond = unitsPerSecond,
                FocalElevation = lookElevation,
                SpeedThreshold = speedThreshold,
                SlowBoomDepth = minBoomDistance,
                SlowBoomElevation = minBoomHeight,
                FastBoomDepth = maxBoomDistance,
                FastBoomElevation = maxBoomHeight
            };
        #endregion
    }
}
