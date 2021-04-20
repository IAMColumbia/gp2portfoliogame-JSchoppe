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
    public partial struct Vector3
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
        /// <summary>
        /// The z axis of the vector.
        /// </summary>
        public float z;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new Vector3 from the given axis values.
        /// </summary>
        /// <param name="x">The value on the x axis.</param>
        /// <param name="y">The value on the y axis.</param>
        /// <param name="z">The value on the z axis.</param>
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        #endregion
        #region Factory Accessors
        /// <summary>
        /// Gets the vector pointing towards the world up direction.
        /// </summary>
        /// <returns>A unit vector pointing up.</returns>
        public static partial Vector3 Up();
        #endregion
        #region Method Headers
        /// <summary>
        /// Calculates a Vector in the same direction with length 1.
        /// </summary>
        /// <returns>A new vector of length 1.</returns>
        public partial Vector3 GetNormalized();
        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        /// <returns>The squared length.</returns>
        public partial float GetLengthSquared();
        #endregion
        #region Operators
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
            => new Vector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
            => new Vector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        public static Vector3 operator *(Vector3 lhs, float rhs)
            => new Vector3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        public static Vector3 operator *(float lhs, Vector3 rhs)
            => new Vector3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
        #endregion
    }
    #endregion
    #region Engine Independent Implementation
    public partial struct Vector3
    {
        public partial float GetLengthSquared()
        {
            return x * x + y * y + z * z;
        }
    }
    #endregion
#if ENGINE_UNITY
    #region Unity Implementation
    public partial struct Vector3
    {
        // These operators handle interoperability.
        public static implicit operator Vector3(UnityEngine.Vector3 unityV3)
            => new Vector3(unityV3.x, unityV3.y, unityV3.z);
        public static implicit operator UnityEngine.Vector3(Vector3 coreV3)
            => new UnityEngine.Vector3(coreV3.x, coreV3.y, coreV3.z);
        // Adapt existing methods.
        public partial Vector3 GetNormalized()
            => ((UnityEngine.Vector3)this).normalized;
        public static partial Vector3 Up()
            => UnityEngine.Vector3.up;
    }
    #endregion
#endif
#if ENGINE_MONOGAME
    #region MonoGame Implementation
    public partial struct MyVector2
    {
        
    }
    #endregion
#endif
}
