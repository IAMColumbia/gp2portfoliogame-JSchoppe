using GameLibrary;
using GameLibrary.Math;
using GameLibrary.Transforms;
using BruteDriveCore.Vehicles;
using UnityLibrary.TopDown2D; // TODO figure out a better way to remove this dependency.

namespace BruteDriveCore.Cameras
{
    /// <summary>
    /// A camera that follows behind a vehicle.
    /// </summary>
    public sealed class VehicleCamera
    {
        #region Fields
        // Component fields.
        private Vehicle vehicle;
        private readonly IOrientedTransform3D camera;
        private readonly ITickProvider tickProvider;
        // Parameter fields.
        private float animationDegreesPerSecond;
        private float animationUnitsPerSecond;
        private float focalElevation;
        private float speedThreshold;
        private float slowBoomDepth;
        private float slowBoomElevation;
        private float fastBoomDepth;
        private float fastBoomElevation;
        // State fields.
        private float rotationDegrees;
        private UnityEngine.Vector2 boomLocation;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new vehicle camera with the given components.
        /// </summary>
        /// <param name="camera">The object to write out camera positioning and orientation to.</param>
        /// <param name="tickProvider">The tick provider for the behaviour.</param>
        public VehicleCamera(IOrientedTransform3D camera, ITickProvider tickProvider)
        {
            // Set components.
            this.camera = camera;
            this.tickProvider = tickProvider;
            // Set default state to behind the vehicle
            // assuming minimum speed.
            rotationDegrees = 180f;
            boomLocation = new UnityEngine.Vector2(slowBoomDepth, slowBoomElevation);
        }
        #endregion
        #region Vehicle Property
        /// <summary>
        /// The vehicle that this camera is following.
        /// </summary>
        public Vehicle Vehicle
        {
            get => vehicle;
            set
            {
                // Manage whether this camera is ticking or not.
                if (vehicle != null)
                    tickProvider.Tick -= Tick;
                vehicle = value;
                if (vehicle != null)
                    tickProvider.Tick += Tick;
            }
        }
        #endregion
        #region Animation Properties
        /// <summary>
        /// The speed at which the camera spins around the vehicle
        /// when changing directions.
        /// </summary>
        public float AnimationDegreesPerSecond
        {
            get => animationDegreesPerSecond;
            set => animationDegreesPerSecond = FloatMath.Max(0f, value);
        }
        /// <summary>
        /// The speed at which the camera draws in and out
        /// when the vehicle linear speed is changing.
        /// </summary>
        public float AnimationUnitsPerSecond
        {
            get => animationUnitsPerSecond;
            set => animationUnitsPerSecond = FloatMath.Max(0f, value);
        }
        /// <summary>
        /// The elevation of the focal point of the vehicle.
        /// </summary>
        public float FocalElevation
        {
            get => focalElevation;
            set => focalElevation = FloatMath.Max(0f, value);
        }
        /// <summary>
        /// The threshold that is considered moving.
        /// </summary>
        public float SpeedThreshold
        {
            get => speedThreshold;
            set => speedThreshold = FloatMath.Max(0f, value);
        }
        /// <summary>
        /// The distance of the boom from the vehicle
        /// at and below the speed threshold.
        /// </summary>
        public float SlowBoomDepth
        {
            get => slowBoomDepth;
            set => slowBoomDepth = FloatMath.Max(0f, value);
        }
        /// <summary>
        /// The elevation of the boom from the vehicle
        /// at and below the speed threshold.
        /// </summary>
        public float SlowBoomElevation
        {
            get => slowBoomElevation;
            set => slowBoomElevation = FloatMath.Max(0f, value);
        }
        /// <summary>
        /// The distance of the boom from the vehicle
        /// at the maximum speed.
        /// </summary>
        public float FastBoomDepth
        {
            get => fastBoomDepth;
            set => fastBoomDepth = FloatMath.Max(0f, value);
        }
        /// <summary>
        /// The elevation of the boom from the vehicle
        /// at the maximum speed.
        /// </summary>
        public float FastBoomElevation
        {
            get => fastBoomElevation;
            set => fastBoomElevation = FloatMath.Max(0f, value);
        }
        #endregion
        #region Camera Tick Routine
        private void Tick(float deltaTime)
        {
            // Get an interpolant for how fast the vehicle
            // is moving over its max speed.
            float speedInterpolant = FloatMath.Clamp(
                FloatMath.InverseLerp(
                    speedThreshold,
                    Vehicle.ForwardsMaxSpeed,
                    FloatMath.Abs(Vehicle.Speed))
                , 0f, 1f);
            // Ease towards where the boom should be locally.
            UnityEngine.Vector2 boomTarget = new UnityEngine.Vector2(
                FloatMath.Lerp(slowBoomDepth, fastBoomDepth, speedInterpolant),
                FloatMath.Lerp(slowBoomElevation, fastBoomElevation, speedInterpolant));
            boomLocation = UnityEngine.Vector2.MoveTowards(boomLocation,
                boomTarget, deltaTime * animationUnitsPerSecond);
            // Is the vehicle traveling in a negative
            // direction? If so pivot the camera behind
            // the backside of the vehicle.
            float targetDegrees =
                (Vehicle.Speed < -speedThreshold) ? 0f : 180f;
            // Ease gradually.
            rotationDegrees = FloatMath.MoveTowards(rotationDegrees,
                targetDegrees, deltaTime * animationDegreesPerSecond);
            // Post the transform changes to the camera.
            Vector3 target = ((UnityEngine.Vector2)Vehicle.Location).TopDownUnflatten();
            camera.Position = target + new Vector3(
                FloatMath.SinDeg(Vehicle.Angle + rotationDegrees) * boomLocation.x,
                FloatMath.Lerp(slowBoomElevation, boomLocation.y, speedInterpolant),
                FloatMath.CosDeg(Vehicle.Angle + rotationDegrees) * boomLocation.x);
            camera.Forwards = (target + Vector3.Up() * focalElevation) - camera.Position;
        }
        #endregion
    }
}
