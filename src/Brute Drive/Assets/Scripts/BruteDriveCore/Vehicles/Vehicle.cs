using GameLibrary;
using GameLibrary.CSharpExtensions;
using UnityEngine; // Depends on Mathf and Vector2.

namespace BruteDriveCore.Vehicles
{
    #region Handlers
    /// <summary>
    /// Handler delegate for when a vehicle is destroyed.
    /// Passes through the vehicle that has been destroyed.
    /// </summary>
    /// <param name="vehicle">The destroyed vehicle.</param>
    public delegate void VehicleDestroyedHandler(Vehicle vehicle);
    #endregion
    /// <summary>
    /// Implements the base vehicle simulation.
    /// </summary>
    public class Vehicle : ITickable
    {
        #region Fields
        // Required Components.
        protected readonly ITickProvider tickProvider;
        // Property State.
        private float ambientFriction;
        private float maxSteerFriction;
        private float steerDegreesPerSecond;
        private float maxSteerAngle;
        private float steerRadiusFactor;
        private float forwardsAcceleration;
        private float reverseAcceleration;
        private float forwardsMaxSpeed;
        private float reverseMaxSpeed;
        private float health;
        private float impactResistance;
        // Internal State.
        bool hasDestroyed;
        private float angle;
        private float speed;
        private float steerAngle;
        private Vector2 forwards;
        #endregion
        #region Broadcasters
        /// <summary>
        /// Called when this vehicle has been destroyed.
        /// </summary>
        public event VehicleDestroyedHandler Destroyed;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new vehicle with the given tick provider.
        /// </summary>
        /// <param name="tickProvider">The behaviour that provides tick signals to this vehicle.</param>
        public Vehicle(ITickProvider tickProvider)
        {
            // Subscribe to Tick.
            this.tickProvider = tickProvider;
            tickProvider.Tick += Tick;
            // Set initial state values.
            speed = 0f;
            angle = 0f;
            steerAngle = 0f;
            forwards = Vector2.up;
            hasDestroyed = false;
            // Set initial property values.
            ambientFriction = 0f;
            maxSteerFriction = 1f;
            steerDegreesPerSecond = 90f;
            maxSteerAngle = 40f;
            forwardsAcceleration = 5f;
            reverseAcceleration = 3f;
            forwardsMaxSpeed = 10f;
            reverseMaxSpeed = 4f;
            health = 1f;
            impactResistance = 0f;
        }
        #endregion
        #region Properties Friction
        /// <summary>
        /// The amount of natural friction deceleration slowing
        /// the vehicle to a stop in units per second squared.
        /// </summary>
        public float AmbientFriction
        {
            get => ambientFriction;
            set => ambientFriction = Mathf.Max(0f, value);
        }
        /// <summary>
        /// The added friction deceleration when the vehicle's
        /// wheels are turned to their max limit. In units
        /// per second squared.
        /// </summary>
        public float MaxSteerFriction
        {
            get => maxSteerFriction;
            set => maxSteerFriction = Mathf.Max(0f, value);
        }
        #endregion
        #region Properties Steering
        /// <summary>
        /// Controls how rapidly the wheels can turn.
        /// Increasing this value makes steering input weightier.
        /// </summary>
        public float SteerDegreesPerSecond
        {
            get => steerDegreesPerSecond;
            set => steerDegreesPerSecond = Mathf.Max(0f, value);
        }
        /// <summary>
        /// Controls the max angle in degrees that the wheels
        /// will turn to.
        /// </summary>
        public float MaxSteerAngle
        {
            get => maxSteerAngle;
            set => maxSteerAngle = Mathf.Max(0f, value);
        }
        /// <summary>
        /// Controls how wide the vehicle will turn relative to its speed.
        /// </summary>
        public float SteerRadiusFactor
        {
            get => steerRadiusFactor;
            set => steerRadiusFactor = Mathf.Max(float.Epsilon, value);
        }
        #endregion
        #region Properties Acceleration
        /// <summary>
        /// Controls the acceleration in units per second squared
        /// when the vehicle is in drive.
        /// </summary>
        public float ForwardsAcceleration
        {
            get => forwardsAcceleration;
            set => forwardsAcceleration = Mathf.Max(0f, value);
        }
        /// <summary>
        /// Controls the acceleration in units per second squared
        /// when the vehicle is in reverse.
        /// </summary>
        public float ReverseAcceleration
        {
            get => reverseAcceleration;
            set => reverseAcceleration = Mathf.Max(0f, value);
        }
        /// <summary>
        /// The maximum vehicle speed when in drive.
        /// </summary>
        public float ForwardsMaxSpeed
        {
            get => forwardsMaxSpeed;
            set => forwardsMaxSpeed = Mathf.Max(0f, value);
        }
        /// <summary>
        /// The maximum vehicle speed when in reverse.
        /// </summary>
        public float ReverseMaxSpeed
        {
            get => reverseMaxSpeed;
            set => reverseMaxSpeed = Mathf.Max(0f, value);
        }
        #endregion
        #region Properties Damage
        /// <summary>
        /// The current health of the vehicle.
        /// This is a number between 0-1.
        /// </summary>
        public float Health
        {
            get => health;
            set
            {
                // Update value and rendered health.
                health = Mathf.Clamp(value, 0f, 1f);
                if (Renderer != null)
                    Renderer.Health = health;
                // Check if the vehicle should be destroyed.
                if (health == 0f && !hasDestroyed)
                    Destroy();
            }
        }
        /// <summary>
        /// Any force magnitude below this number will be ignored.
        /// </summary>
        public float ImpactResistance
        {
            get => impactResistance;
            set => impactResistance = Mathf.Max(0f, value);
        }
        #endregion
        #region Properties Movement
        /// <summary>
        /// The speed of the vehicle in units per second.
        /// If in reverse this value will be negative.
        /// </summary>
        public float Speed
        {
            get => speed;
            set => speed = Mathf.Clamp(
                value, -ReverseMaxSpeed, ForwardsMaxSpeed);
        }
        /// <summary>
        /// The angle of the vehicle in degrees.
        /// </summary>
        public float Angle
        {
            get => angle;
            set
            {
                // Keep the angle between 0-360.
                angle = value.WrappedBetween(0f, 360f);
                // Update forwards.
                forwards = new Vector2(
                    Mathf.Sin(angle * Mathf.Deg2Rad),
                    Mathf.Cos(angle * Mathf.Deg2Rad));
            }
        }
        /// <summary>
        /// The current top down location of the vehicle in units.
        /// </summary>
        public Vector2 Location { get; set; }
        /// <summary>
        /// The velocity vector of the vehicle in units per second.
        /// </summary>
        public Vector2 Velocity => forwards * speed;
        #endregion
        #region Properties Optional Components
        /// <summary>
        /// Optional component that controls the vehicle.
        /// </summary>
        public IVehicleController Controller { get; set; }
        /// <summary>
        /// Optional component that renders the vehicle.
        /// </summary>
        public IVehicleRenderer Renderer { get; set; }
        /// <summary>
        /// Performs hit scans on the physics space during movement.
        /// </summary>
        public IVehicleCollider Collider { get; set; }
        #endregion
        #region Methods Impact
        /// <summary>
        /// Applies an impact to this vehicle.
        /// If the impact is strong enough it will damage the vehicle.
        /// </summary>
        /// <param name="impactVector">The direction and magnitude of the impact.</param>
        public void ApplyImpact(Vector2 impactVector)
        {
            
        }
        /// <summary>
        /// Reduces health to zero, and renders this vehicle uncontrollable.
        /// Will be pushed around by other bodies.
        /// </summary>
        public void Destroy()
        {
            if (!hasDestroyed)
            {
                hasDestroyed = true;
                Destroyed?.Invoke(this);
            }
        }
        #endregion
        #region Simulation Tick
        /// <summary>
        /// Ticks the simulation of the vehicle.
        /// </summary>
        /// <param name="deltaTime">The time step for this tick.</param>
        public void Tick(float deltaTime)
        {
            // Process input if there is a controller (optional).
            if (Controller != null) ProcessInput(deltaTime);
            // Update the position of the vehicle.
            UpdatePosition(deltaTime);
            // Update displayed state if there is a renderer (optional).
            if (Renderer != null) UpdateRenderer();
        }
        private void ProcessInput(float deltaTime)
        {
            // Where does the controller want the
            // tires to be facing:
            float steerTarget = Controller.SteeringAngle * maxSteerAngle;
            // How much can the tires turn?
            float steerDelta = steerDegreesPerSecond * deltaTime;
            // Move towards the desired steer direction,
            // making sure not to overshoot it.
            if (steerTarget > steerAngle)
                steerAngle = Mathf.Min(steerAngle + steerDelta, steerTarget);
            else if (steerTarget < steerAngle)
                steerAngle = Mathf.Max(steerAngle - steerDelta, steerTarget);
            // Enforce limit on steering.
            steerAngle = Mathf.Clamp(steerAngle, -maxSteerAngle, maxSteerAngle);
            // Apply steering to the angle of the vehicle.
            // This is done relative to the current speed of the
            // vehicle to prevent pivoting in place.
            Angle += ((Speed > 0f)? 1f : -1f) *
                steerAngle * deltaTime * Speed / steerRadiusFactor;
            // Apply the change in speed from the controller pedals.
            Speed += deltaTime * (
                Controller.GasPedalAmount * ForwardsAcceleration -
                Controller.BrakePedalAmount * ReverseAcceleration);
        }
        private void UpdatePosition(float deltaTime)
        {
            // Apply friction to speed before moving.
            float friction = deltaTime *
                (ambientFriction +
                Mathf.Abs(steerAngle) / maxSteerAngle * maxSteerFriction);
            if (Speed > 0f)
                Speed = Mathf.Max(0f, speed - friction);
            else
                Speed = Mathf.Min(0f, speed + friction);
            // Move the body along its forward path.
            if (Collider == null)
                Location += forwards * deltaTime * Speed;
            // If there is a collider use a sweep.
            else if (Collider.MoveSweep(forwards * deltaTime * Speed, out Vehicle hitVehicle))
            {

            }
        }
        private void UpdateRenderer()
        {
            Renderer.Position = Location;
            Renderer.Forwards = forwards;
            // Reflect wheel direction based on
            // whether we are reversing.
            Renderer.SteerAngle = (speed > 0f) ?
                steerAngle : -steerAngle;
        }
        #endregion
    }
}
