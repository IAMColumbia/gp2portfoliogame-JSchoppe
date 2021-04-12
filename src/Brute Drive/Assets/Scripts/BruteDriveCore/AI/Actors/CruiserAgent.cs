using GameLibrary;
using GameLibrary.StateMachines;
using BruteDriveCore.Vehicles;
using UnityEngine;

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
            Charging
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
            private bool charging;
            #endregion
            #region Constructors
            /// <summary>
            /// Creates a new charging state for the given cruiser.
            /// </summary>
            /// <param name="agent"></param>
            public ChargingState(CruiserAgent agent)
            {
                this.agent = agent;
                // Set default values.
                DesiredDistance = 10f;
                charging = false;
            }
            #endregion
            #region Properties
            /// <summary>
            /// Controls how far the cruiser wants to be before starting a charge.
            /// </summary>
            public float DesiredDistance { get; set; }
            #endregion

            public void StateEntered()
            {
                
            }
            public void StateExited()
            {
                
            }

            public void Tick(float deltaTime)
            {
                // Steer towards the target.
                float angle = Vector2.SignedAngle(agent.vehicle.Velocity, agent.Target.Location - agent.vehicle.Location);

                if (angle > 0f)
                {
                    agent.SteeringAngle = -1f * (angle / 180f);
                }
                else if (angle < 0f)
                {
                    agent.SteeringAngle = 1f * (angle / 180f);
                }

                agent.GasPedalAmount = 1f;

                if (charging)
                {

                }
                else
                {
                    if (Vector2.SqrMagnitude(agent.vehicle.Location - agent.Target.Location)
                        > DesiredDistance * DesiredDistance)
                    {

                    }
                    else
                    {

                    }
                }
            }
        }
        #endregion
    }
}
