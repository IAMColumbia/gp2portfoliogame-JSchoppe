using System;
using System.Collections.Generic;

namespace BruteDrive.AICore
{
    /// <summary>
    /// Base class for finite state machines.
    /// </summary>
    /// <typeparam name="TStateKey">The key used to identify the state.</typeparam>
    public abstract class StateMachine<TStateKey> : ITickable
    {
        #region State Fields
        protected Dictionary<TStateKey, IState> states;
        private TStateKey currentState;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new empty state machine.
        /// </summary>
        public StateMachine()
        {
            states = new Dictionary<TStateKey, IState>();
        }
        #endregion
        #region Add States
        /// <summary>
        /// Adds a new state to the state machine.
        /// If one already exists it will be overwritten.
        /// </summary>
        /// <param name="state">The state identifier.</param>
        /// <param name="implementation">The state implementation.</param>
        public void AddState(TStateKey state, IState implementation)
        {
            if (CurrentState.Equals(state))
            {
                // Exit out of the state and reimplement the
                // new version of the state to enter.
                this[currentState].StateExited();
                states[state] = implementation;
                this[currentState].StateEntered();
            }
            else if (states.ContainsKey(state))
                // Reimplement the state.
                states[state] = implementation;
            else
                // Add the state.
                states.Add(state, implementation);
        }
        #endregion
        #region State Events
        /// <summary>
        /// Called every time this machine changes state.
        /// </summary>
        public event Action<TStateKey> StateChanged;
        #endregion
        #region State Properties
        /// <summary>
        /// The state that the machine is currently in.
        /// </summary>
        public TStateKey CurrentState
        {
            get => currentState;
            set
            {
                // Is this a new state?
                if (!currentState.Equals(value))
                {
                    // Exit current state.
                    states[currentState].StateExited();
                    currentState = value;
                    // Enter new state.
                    states[currentState].StateEntered();
                    // Notify listeners.
                    StateChanged?.Invoke(currentState);
                }
            }
        }
        #endregion
        #region Protected State Accessor
        /// <summary>
        /// Retrieves a state with the given state key.
        /// </summary>
        /// <param name="key">The state key.</param>
        /// <returns>The IStateMachine state object.</returns>
        protected IState this[TStateKey key] => states[key];
        #endregion
        #region Tick Routine
        /// <summary>
        /// Ticks the current state in the machine if applicable.
        /// </summary>
        /// <param name="deltaTime">The elapsed time in seconds.</param>
        public virtual void Tick(float deltaTime)
        {
            // Invoke the current state's tick if applicable.
            if (this[currentState] is ITickable state)
                state.Tick(deltaTime);
        }
        #endregion
    }
}
