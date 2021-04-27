// TODO engine configuration should be centralized.
// Toggle based on the engine.
#define ENGINE_UNITY
/*
#define ENGINE_MONOGAME
*/

namespace GameLibrary.Math
{
    #region Struct Header
    /// <summary>
    /// Contains a pair of values representing a point in 2D space.
    /// </summary>
    public partial struct Vector2
    {
        #region Fields
        /// <summary>
        /// The x axis of the vector.
        /// </summary>
        public float x;
        /// <summary>
        /// The y axis of the vector.
        /// </summary>
        public float y;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new Vector2 from the given axis values.
        /// </summary>
        /// <param name="x">The value on the x axis.</param>
        /// <param name="y">The value on the y axis.</param>
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion
        #region Method Headers
        /// <summary>
        /// Gets the magnitude of this vector.
        /// </summary>
        /// <returns>The directionless length of the vector.</returns>
        public partial float GetLength();
        /// <summary>
        /// Calculates a Vector in the same direction with length 1.
        /// </summary>
        /// <returns>A new vector of length 1.</returns>
        public partial Vector2 GetNormalized();
        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        /// <returns>The squared length.</returns>
        public partial float GetLengthSquared();
        /// <summary>
        /// Gets the degrees angle of this vector relative to Y-up.
        /// </summary>
        /// <returns>The angle between Y-up and the vector from 0-360 degrees.</returns>
        public partial float GetDegrees();
        #endregion
        #region Operators
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
            => new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
            => new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
        public static Vector2 operator *(Vector2 lhs, float rhs)
            => new Vector2(lhs.x * rhs, lhs.y * rhs);
        public static Vector2 operator *(float lhs, Vector2 rhs)
            => new Vector2(lhs * rhs.x, lhs * rhs.y);
        #endregion
    }
    #endregion
    #region Engine Independent Implementation
    public partial struct Vector2
    {
        public partial float GetLength()
        {
            return FloatMath.Sqrt(x * x + y * y);
        }
        public partial float GetLengthSquared()
        {
            return x * x + y * y;
        }
    }
    #endregion
#if ENGINE_UNITY
    #region Unity Implementation
    public partial struct Vector2
    {
        // These operators handle interoperability.
        public static implicit operator Vector2(UnityEngine.Vector2 unityV2)
            => new Vector2(unityV2.x, unityV2.y);
        public static implicit operator UnityEngine.Vector2(Vector2 coreV2)
            => new UnityEngine.Vector2(coreV2.x, coreV2.y);
        public static implicit operator UnityEngine.Vector3(Vector2 coreV2)
            => new UnityEngine.Vector3(coreV2.x, coreV2.y);
        // Adapt existing methods.
        public partial Vector2 GetNormalized()
            => ((UnityEngine.Vector2)this).normalized;
        public partial float GetDegrees()
            => UnityEngine.Mathf.Atan2(x, y) * FloatMath.RAD_2_DEG;
    }
    #endregion
#endif
#if ENGINE_MONOGAME
    #region MonoGame Implementation
    public partial struct MyVector2
    {
        public static implicit operator MyVector2(Microsoft.Xna.Framework.Vector2 monogameV2)
            => new MyVector2() { x = monogameV2.x, y = monogameV2.y };
    }
    #endregion
#endif
}
