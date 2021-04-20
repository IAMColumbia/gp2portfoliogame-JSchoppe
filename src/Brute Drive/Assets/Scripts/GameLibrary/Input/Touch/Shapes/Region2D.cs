using GameLibrary.Math;

namespace GameLibrary.Input.Touch.Shapes
{
    /// <summary>
    /// Base class for all regions.
    /// </summary>
    public abstract class Region2D : IRegion2D
    {
        #region Region Methods
        /// <summary>
        /// Checks to see if a point is inside the region, offsetting region edges.
        /// </summary>
        /// <param name="position">The point to test.</param>
        /// <param name="offset">The amount to offset the region boundaries by.</param>
        /// <returns>True if the point is inside the region.</returns>
        public abstract bool CheckInside(Vector2 position, float offset);
        /// <summary>
        /// Checks to see if a point is inside the region.
        /// </summary>
        /// <param name="position">The point to test.</param>
        /// <returns>True if the point is inside the region.</returns>
        public bool CheckInside(Vector2 position)
            => CheckInside(position, 0f);
        #endregion
    }
}
