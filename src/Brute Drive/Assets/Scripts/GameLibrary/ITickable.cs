namespace GameLibrary
{
    /// <summary>
    /// Defines an object that can be ticked.
    /// </summary>
    public interface ITickable
    {
        #region Methods Implemented
        /// <summary>
        /// Method that is called on the tick routine.
        /// </summary>
        /// <param name="deltaTime">The elapsed time in seconds since the last tick.</param>
        void Tick(float deltaTime);
        #endregion
    }
}
