using GameLibrary.Math;
using GameLibrary.Input.Touch.Shapes;

namespace GameLibrary.Input.Touch
{
    // TODO this class implements expanding behaviour
    // by default. This behaviour should be optional;
    // architecture should be rethought here.
    // TODO this class is lacking in UX options for
    // how the dial should reset after hover focus is lost.
    // TODO a dial control should be able to have limits.
    #region Handler Delegates
    /// <summary>
    /// Listener for when an input control dial changes its angle.
    /// </summary>
    /// <param name="newAngle">The new angle of the dial.</param>
    public delegate void DialAngleChangedListener(float newAngle);
    #endregion
    /// <summary>
    /// Implements a dial touch control.
    /// </summary>
    public sealed class DialHoverTouchControl : ExpandingHoverTouchControl, ITouchHoverTickListener
    {
        #region State Broadcasters
        /// <summary>
        /// Called whenever the dial angle changes.
        /// </summary>
        public event DialAngleChangedListener DialAngleChanged;
        #endregion
        #region Fields
        private float angleOffset;
        private float currentAngle;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new dial hover control with the given region.
        /// </summary>
        /// <param name="region">The region of the control.</param>
        public DialHoverTouchControl(IRegion2D region) : base(region)
        {

        }
        #endregion
        #region Properties
        /// <summary>
        /// The current angle of the control in degrees
        /// relative to the Y-up axis.
        /// </summary>
        public float CurrentAngle => currentAngle;
        #endregion
        #region Hover Listener Extended Implementation
        public override void HoverStarted(Vector2 hoverEnter)
        {
            // Localize the value.
            hoverEnter -= OriginLocation;
            base.HoverStarted(hoverEnter);
            // The pivot will be relative to the starting angle.
            angleOffset = hoverEnter.GetDegrees();
        }
        public override void HoverExited()
        {
            base.HoverExited();
            // Reset the dial back to zero.
            angleOffset = 0f;
            DialAngleChanged?.Invoke(0f);
        }
        public void HoverTick(float deltaTime, Vector2 position)
        {
            // Localize the value.
            position -= OriginLocation;
            // Check and post the new angle value.
            float newAngle = position.GetDegrees() - angleOffset;
            if (newAngle < 0f)
                newAngle += 360f;
            DialAngleChanged?.Invoke(newAngle);
        }
        #endregion
    }
}
