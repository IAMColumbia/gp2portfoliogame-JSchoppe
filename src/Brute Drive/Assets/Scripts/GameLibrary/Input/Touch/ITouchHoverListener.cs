using UnityEngine; // TODO adapt Vector2.

namespace GameLibrary.Input.Touch
{
    /// <summary>
    /// Implements behavior for when a touch motion is hovered
    /// over this object. Can only register one touch hover motion
    /// at a time (others will be ignored).
    /// </summary>
    public interface ITouchHoverListener : ITouchCastable
    {
        // TODO tick method could probably be further
        // abstracted for more specific interfaces.
        #region Methods Implemented
        /// <summary>
        /// Called when a touch begins hovering over the listener.
        /// </summary>
        /// <param name="hoverEnter">The screen coordinates where hovering started.</param>
        void HoverStarted(Vector2 hoverEnter);
        /// <summary>
        /// Called each tick while the touch motion is still hovering.
        /// </summary>
        /// <param name="deltaTime">The elapsed time for this tick.</param>
        /// <param name="position">The current screen coordinates of the touch.</param>
        void HoverTick(float deltaTime, Vector2 position);
        /// <summary>
        /// Called when the touch stops hovering over the listener.
        /// </summary>
        void HoverExited();
        #endregion
    }
}
