namespace GameLibrary
{
    #region Event Delegates
    /// <summary>
    /// Function that responds to ticking routines.
    /// </summary>
    /// <param name="deltaTime">The change in time in seconds.</param>
    public delegate void TickListener(float deltaTime);
    #endregion
    /// <summary>
    /// Implements tick events that pass time deltas.
    /// </summary>
    public interface ITickProvider
    {
        #region Events Implemented
        /// <summary>
        /// Called on tick as specified by the provider.
        /// Passes change in time since last tick.
        /// </summary>
        event TickListener Tick;
        #endregion
    }
}
