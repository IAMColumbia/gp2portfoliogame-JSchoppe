using GameLibrary.Math;

namespace GameLibrary.Input.Touch.Shapes
{
    /// <summary>
    /// Implements a circle hitscan region.
    /// </summary>
    public sealed class CircleRegion2D : Region2D
    {
        #region Fields
        private float radiusSquared;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new circular region with the given radius.
        /// </summary>
        /// <param name="radius">The radius of the region.</param>
        public CircleRegion2D(float radius)
        {
            Radius = radius;
        }
        #endregion
        #region Properties
        /// <summary>
        /// The radius of the region.
        /// </summary>
        public float Radius
        {
            get => FloatMath.Sqrt(radiusSquared);
            set => radiusSquared = value * value;
        }
        #endregion
        #region Hitscan Implementation
        /// <summary>
        /// Checks if the given point is inside the circle.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <param name="offset">The offset of the circle radius.</param>
        /// <returns>True if the point is inside the region.</returns>
        public override bool CheckInside(Vector2 position, float offset)
        {
            // Look at square magnitude difference.
            return
                position.x * position.x + position.y * position.y
                <=
                radiusSquared + offset * offset * ((offset < 0f)? -1f : 1f);
        }
        #endregion
    }
}
