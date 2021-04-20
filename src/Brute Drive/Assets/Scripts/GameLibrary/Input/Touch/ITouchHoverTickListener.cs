using GameLibrary.Math;

namespace GameLibrary.Input.Touch
{
    /// <summary>
    /// Implements behavior for when a touch motion is hovered
    /// over this object on tick. Registers as long as at least one
    /// touch is hovered over the object.
    /// </summary>
    public interface ITouchHoverTickListener
    {
        #region Methods Implemented
        /// <summary>
        /// Called each tick while the touch motion is still hovering.
        /// </summary>
        /// <param name="deltaTime">The elapsed time for this tick.</param>
        /// <param name="position">The current screen coordinates of the touch.</param>
        void HoverTick(float deltaTime, Vector2 position);
        #endregion
    }
}
