using GameLibrary.Math;

namespace GameLibrary.Input.Touch
{
    // TODO this could be much more DRY. Perhaps the cast geometry
    // shape should be an injected dependency instead of defining the class.
    /// <summary>
    /// A touch control with a circular cast region.
    /// </summary>
    public abstract class CircularTouchControl : TouchControl
    {
        #region Fields
        private float radiusSquared;
        #endregion
        #region TouchCast Implementation
        /// <summary>
        /// Checks whether a screen coordinate is within the circle of this control.
        /// </summary>
        /// <param name="screenPosition">The screen position in pixels.</param>
        /// <returns>True if the touch overlaps the circular region.</returns>
        public override sealed bool TouchCast(Vector2 screenPosition)
        {
            // Is the touched location within the radius
            // of the circle?
            return
                (screenPosition - Location).GetLengthSquared()
                < radiusSquared;
        }
        #endregion
        #region Circle Properties
        /// <summary>
        /// The location of the control in screen space pixels.
        /// </summary>
        public Vector2 Location { get; set; }
        /// <summary>
        /// The radius of the control in pixels.
        /// </summary>
        public float Radius
        {
            get => FloatMath.Sqrt(radiusSquared);
            set
            {
                // Disallow radius of less than zero.
                value = FloatMath.Max(value, 0f);
                radiusSquared = value * value;
            }
        }
        #endregion
    }
}
