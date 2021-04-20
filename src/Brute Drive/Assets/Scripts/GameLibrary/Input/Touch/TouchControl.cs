using GameLibrary.Math;
using GameLibrary.Input.Touch.Shapes;

namespace GameLibrary.Input.Touch
{
    /// <summary>
    /// Base class for all touch controls.
    /// </summary>
    public abstract class TouchControl : ITouchCastable
    {
        #region Constructors
        /// <summary>
        /// Sets default values for the base touch control.
        /// </summary>
        public TouchControl(IRegion2D region)
        {
            Region = region;
            // Set default values.
            IsEnabled = true;
        }
        #endregion
        #region Base Control Properties
        /// <summary>
        /// Whether this object should currently be cast against.
        /// </summary>
        public virtual bool IsEnabled { get; set; }
        /// <summary>
        /// The screen location of this touch control.
        /// </summary>
        public Vector2 OriginLocation { get; set; }
        /// <summary>
        /// The region that is associated with this control.
        /// </summary>
        public IRegion2D Region { get; set; }
        #endregion
        #region ITouchCastable Default Implementation
        /// <summary>
        /// Checks whether a screen coordinate is on this object.
        /// </summary>
        /// <param name="screenPosition">The screen position in pixels.</param>
        /// <returns>True if the touch overlaps the touchable object.</returns>
        public virtual bool TouchCast(Vector2 screenPosition)
            => Region.CheckInside(screenPosition - OriginLocation);
        #endregion
    }
}
