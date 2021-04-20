namespace GameLibrary.StateMachines
{
    /// <summary>
    /// Defines the implementation required to work with the
    /// state machine model.
    /// </summary>
    public interface IState
    {
        #region Method Requirements
        /// <summary>
        /// Called when a state is entered.
        /// </summary>
        void StateEntered();
        /// <summary>
        /// Called when this state is exited.
        /// </summary>
        void StateExited();
        #endregion
    }
}
