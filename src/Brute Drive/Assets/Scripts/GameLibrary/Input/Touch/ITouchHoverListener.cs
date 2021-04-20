using GameLibrary.Math;

namespace GameLibrary.Input.Touch
{
    /// <summary>
    /// Implements behavior for when a touch motion is hovered
    /// over this object. Registers as long as at least one
    /// touch is hovered over the object.
    /// </summary>
    public interface ITouchHoverListener : ITouchCastable
    {
        #region Methods Implemented
        /// <summary>
        /// Called when a touch begins hovering over the listener.
        /// </summary>
        /// <param name="hoverEnter">The screen coordinates where hovering started.</param>
        void HoverStarted(Vector2 hoverEnter);
        /// <summary>
        /// Called when the touch stops hovering over the listener.
        /// </summary>
        void HoverExited();
        #endregion
    }
}
