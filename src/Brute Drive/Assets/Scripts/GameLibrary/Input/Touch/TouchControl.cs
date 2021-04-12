using UnityEngine; // TODO adapt Vector2.

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
        public TouchControl()
        {
            // Set default values.
            IsEnabled = true;
        }
        #endregion
        #region Base Control Properties
        /// <summary>
        /// Whether this object should currently be cast against.
        /// </summary>
        public virtual bool IsEnabled { get; set; }
        #endregion
        #region Subclass ITouchCastable Requirements
        /// <summary>
        /// Checks whether a screen coordinate is on this object.
        /// </summary>
        /// <param name="screenPosition">The screen position in pixels.</param>
        /// <returns>True if the touch overlaps the touchable object.</returns>
        public abstract bool TouchCast(Vector2 screenPosition);
        #endregion
    }
}
