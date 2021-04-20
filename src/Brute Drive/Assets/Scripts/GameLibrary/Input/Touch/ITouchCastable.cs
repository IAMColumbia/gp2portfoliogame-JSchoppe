using GameLibrary.Math;

namespace GameLibrary.Input.Touch
{
    /// <summary>
    /// Implements a method for checking if a touch is on this object.
    /// </summary>
    public interface ITouchCastable
    {
        #region Methods Implemented
        /// <summary>
        /// Checks whether a screen coordinate is on this object.
        /// </summary>
        /// <param name="screenPosition">The screen position in pixels.</param>
        /// <returns>True if the touch overlaps the touchable object.</returns>
        bool TouchCast(Vector2 screenPosition);
        #endregion
    }
}
