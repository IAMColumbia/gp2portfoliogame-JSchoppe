using GameLibrary.Math;
using GameLibrary.Input.Touch.Shapes;

namespace GameLibrary.Input.Touch
{
    #region Handler Delegates
    /// <summary>
    /// A listener that responds to changes in hovered state.
    /// </summary>
    /// <param name="isHovered">The new hovered state of the control.</param>
    public delegate void HoverStateChangedListener(bool isHovered);
    #endregion
    /// <summary>
    /// Implements a control that can be hovered over.
    /// This control type does not require direct tapping.
    /// </summary>
    public class HoverTouchControl : TouchControl, ITouchHoverListener
    {
        #region State Broadcasters
        /// <summary>
        /// Called when the hover state of this object changes.
        /// </summary>
        public event HoverStateChangedListener HoverStateChanged;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new hover touch control with the given region.
        /// </summary>
        /// <param name="region">The region of the control.</param>
        public HoverTouchControl(IRegion2D region) : base(region)
        {

        }
        #endregion
        #region Hover Listening
        /// <summary>
        /// Called when a touch begins hovering over the listener.
        /// </summary>
        /// <param name="hoverEnter">The screen coordinates where hovering started.</param>
        public virtual void HoverStarted(Vector2 hoverEnter)
        {
            HoverStateChanged?.Invoke(true);
        }
        /// <summary>
        /// Called when the touch stops hovering over the listener.
        /// </summary>
        public virtual void HoverExited()
        {
            HoverStateChanged?.Invoke(false);
        }
        #endregion
    }
}
