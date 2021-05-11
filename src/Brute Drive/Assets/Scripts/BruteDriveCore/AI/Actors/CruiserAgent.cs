using GameLibrary;
using GameLibrary.StateMachines;
using BruteDriveCore.Vehicles;
using UnityEngine;
using GameLibrary.Math;

namespace BruteDriveCore.AI.Actors
{
    /// <summary>
    /// Implements a state machine that controls the cruiser.
    /// This AI behaviour will attempt to destroy the target
    /// </summary>
    public sealed class CruiserAgent : StateMachine<CruiserAgent.State>, IVehicleController
    {
        #region Vehicle Controller Properties
        /// <summary>
        /// The current force the agent is applying to the gas pedal.
        /// </summary>
        public float GasPedalAmount { get; private set; }
        /// <summary>
        /// The current force the agent is applying to the brake pedal.
        /// </summary>
        public float BrakePedalAmount { get; private set; }
        /// <summary>
        /// The current steer angle the agent is applying to the wheel.
        /// </summary>
        public float SteeringAngle { get; private set; }
        #endregion

        private Vehicle vehicle;

        /// <summary>
        /// Defines all states a cruiser agent can enter.
        /// </summary>
        public enum State
        {
            /// <summary>
            /// Agents comes to a stop doing nothing.
            /// </summary>
            Stationary,
            /// <summary>
            /// Agent is traveling to a targeted vehicle.
            /// </summary>
            Traveling,
            /// <summary>
            /// Agent is charging a targeted vehicle.
            /// </summary>
            Charging,
            /// <summary>
            /// Agent is backing off of a targeted vehicle.
            /// </summary>
            BackingOff
        }

        /// <summary>
        /// Creates a new Cruiser Agent with default property values.
        /// </summary>
        /// <param name="vehicle">The vehicle that will be controlled by this agent.</param>
        public CruiserAgent(Vehicle vehicle)
        {
            this.vehicle = vehicle;
            vehicle.Controller = this;
            states.Add(State.Stationary, new DisabledState(this));
            states.Add(State.Charging, new ChargingState(this));
            states.Add(State.BackingOff, new BackingOffState(this));

            vehicle.VehicleHit += OnVehicleHit;
        }

        private void OnVehicleHit(VehicleHitResult hit)
        {
            if (hit.vehicle == Target)
            {
                if (CurrentState == State.Charging)
                {
                    CurrentState = State.BackingOff;
                    (states[State.BackingOff] as BackingOffState).ChargingDistance = 10f;
                }
            }
            else
            {
                if (CurrentState == State.Charging)
                {
                    CurrentState = State.BackingOff;
                    (states[State.BackingOff] as BackingOffState).ChargingDistance = 7f;
                }
            }
        }

        public Vehicle Vehicle => vehicle;

        /// <summary>
        /// Sets the targeted vehicle for this agent.
        /// </summary>
        public Vehicle Target { get; set; }

        #region State Implementations
        private sealed class DisabledState : IState, ITickable
        {
            #region Fields
            private readonly CruiserAgent agent;
            #endregion
            #region Constructors
            /// <summary>
            /// Creates a new disabled state for the given cruiser.
            /// </summary>
            /// <param name="agent"></param>
            public DisabledState(CruiserAgent agent)
            {
                this.agent = agent;
                // Set default values.
                SlowingFalloff = 2f;
            }
            #endregion
            #region Properties
            /// <summary>
            /// Controls the speed that breaking falls off to stop.
            /// </summary>
            public float SlowingFalloff { get; set; }
            #endregion
            #region State Enter / Exit
            public void StateEntered()
            {
                // Set steering to default.
                agent.SteeringAngle = 0f;
            }
            public void StateExited()
            {
                
            }
            #endregion
            #region State Tick
            public void Tick(float deltaTime)
            {
                // Apply gas or brake to slow the
                // agent down to a stop.
                if (agent.vehicle.Speed > 0f)
                    agent.BrakePedalAmount = 
                        Mathf.Min(
                            Mathf.InverseLerp(
                                0f, SlowingFalloff,
                                agent.vehicle.Speed),
                            1f);
                else if (agent.vehicle.Speed < 0f)
                    agent.GasPedalAmount =
                        Mathf.Min(
                            Mathf.InverseLerp(
                                0f, -SlowingFalloff,
                                agent.vehicle.Speed),
                            1f);
            }
            #endregion
        }
        private sealed class ChargingState : IState, ITickable
        {
            #region Fields
            private readonly CruiserAgent agent;
            #endregion
            #region Constructors
            /// <summary>
            /// Creates a new charging state for the given cruiser.
            /// </summary>
            /// <param name="agent"></param>
            public ChargingState(CruiserAgent agent)
            {
                this.agent = agent;
            }
            #endregion

            public void StateEntered()
            {
                agent.BrakePedalAmount = 0f;
            }
            public void StateExited()
            {
                
            }

            public void Tick(float deltaTime)
            {
                // Steer towards the target.
                float angle = UnityEngine.Vector2.SignedAngle(
                    agent.vehicle.Velocity, agent.Target.Location - agent.vehicle.Location);
                agent.SteeringAngle = -angle / 180f;
                agent.GasPedalAmount = 1f;
            }
        }
        private sealed class BackingOffState : IState, ITickable
        {
            #region Fields
            private readonly CruiserAgent agent;
            private float chargingDistanceSquared;
            #endregion
            #region Constructors
            /// <summary>
            /// Creates a new backing off state for the given cruiser.
            /// </summary>
            /// <param name="agent"></param>
            public BackingOffState(CruiserAgent agent)
            {
                this.agent = agent;
                ChargingDistance = 15f;
            }
            #endregion
            #region Properties
            /// <summary>
            /// The distance to back off from the target
            /// to gain speed.
            /// </summary>
            public float ChargingDistance
            {
                get => FloatMath.Sqrt(chargingDistanceSquared);
                set => chargingDistanceSquared = value * value;
            }
            #endregion

            public void StateEntered()
            {
                agent.GasPedalAmount = 0f;
            }
            public void StateExited()
            {
                
            }

            public void Tick(float deltaTime)
            {
                agent.BrakePedalAmount = 1f;
                if ((agent.Vehicle.Location - agent.Target.Location).GetLengthSquared() >
                    chargingDistanceSquared)
                {
                    agent.CurrentState = State.Charging;
                }
            }
        }
        #endregion
    }
}
