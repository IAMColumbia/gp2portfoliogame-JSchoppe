using GameLibrary.Math;
using GameLibrary.Input.Touch.Shapes;

namespace GameLibrary.Input.Touch
{
    /// <summary>
    /// A hover touch control that expands in size once hovered.
    /// Useful for drag controls that provide more tolerance once hovered.
    /// </summary>
    public class ExpandingHoverTouchControl : HoverTouchControl
    {
        #region Fields
        private float expansion;
        private bool isHovered;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new expanding hover touch control with the given region.
        /// </summary>
        /// <param name="region">The region of the control.</param>
        public ExpandingHoverTouchControl(IRegion2D region) : base(region)
        {
            // Set default values.
            expansion = 0f;
            isHovered = false;
        }
        #endregion
        #region Properties
        /// <summary>
        /// The amount that the region is expanded once hovering.
        /// </summary>
        public float HoverExpansion
        {
            get => expansion;
            set => expansion = FloatMath.Max(0f, value);
        }
        #endregion
        #region Touch Casting Method Override
        public override bool TouchCast(Vector2 screenPosition)
        {
            // Incorporate the expansion value into the touch cast,
            // only if the control was hovered over.
            if (isHovered)
                return Region.CheckInside(screenPosition - OriginLocation, expansion);
            else
                return Region.CheckInside(screenPosition - OriginLocation);
        }
        #endregion
        #region Hover Listener Additional Implementation
        public override void HoverStarted(Vector2 hoverEnter)
        {
            base.HoverStarted(hoverEnter);
            isHovered = true;
        }
        public override void HoverExited()
        {
            base.HoverExited();
            isHovered = false;
        }
        #endregion
    }
}
