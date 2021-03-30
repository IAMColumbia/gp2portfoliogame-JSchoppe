namespace BruteDrive.StageGeneration
{
    /// <summary>
    /// Implements listeners for map load events.
    /// </summary>
    public interface IGeneratorListener
    {
        #region Callback Requirements
        /// <summary>
        /// The callback that is called when the map loader succeeds.
        /// </summary>
        void OnLoaded();
        /// <summary>
        /// The callback that is called when the map loader fails.
        /// </summary>
        void OnFailed();
        #endregion
    }
}
