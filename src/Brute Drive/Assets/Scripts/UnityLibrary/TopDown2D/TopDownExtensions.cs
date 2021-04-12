using System.Collections.Generic;
using UnityEngine;

namespace UnityLibrary.TopDown2D
{
    /// <summary>
    /// Provides extension methods for ease of conversion between
    /// 2D space and top down 3D space. This allows for easier processing
    /// of 2D logic that is aligned to the XZ plane.
    /// </summary>
    public static class TopDownExtensions
    {
        #region Flatten/Unflatten
        /// <summary>
        /// Flattens a 3D space vector into top down 2D coordinates.
        /// </summary>
        /// <param name="vector">The vector to flatten.</param>
        /// <returns>The vector with x and z occupying x and y.</returns>
        public static Vector2 TopDownFlatten(this Vector3 vector)
            => new Vector2(vector.x, vector.z);
        /// <summary>
        /// Flattens a collection of 3D space vectors into top down 2D coordinates.
        /// </summary>
        /// <param name="vectors">The vectors to flatten.</param>
        /// <returns>An array of the flattened vectors.</returns>
        public static Vector2[] TopDownFlatten(this IList<Vector3> vectors)
        {
            Vector2[] flattened = new Vector2[vectors.Count];
            for (int i = 0; i < vectors.Count; i++)
                flattened[i] = vectors[i].TopDownFlatten();
            return flattened;
        }
        /// <summary>
        /// Unflattens a 2D space vector into top down 3D space.
        /// </summary>
        /// <param name="vector">The vector to unflatten.</param>
        /// <returns>The vector with x and y occupying x and z.</returns>
        public static Vector3 TopDownUnflatten(this Vector2 vector)
            => new Vector3(vector.x, 0f, vector.y);
        /// <summary>
        /// Unflattens a collection of 2D space vectors into top down 3D space.
        /// </summary>
        /// <param name="vectors">The vectors to unflatten.</param>
        /// <returns>An array of the unflattened vectors.</returns>
        public static Vector3[] TopDownUnflatten(this IList<Vector2> vectors)
        {
            Vector3[] unFlattened = new Vector3[vectors.Count];
            for (int i = 0; i < vectors.Count; i++)
                unFlattened[i] = vectors[i].TopDownUnflatten();
            return unFlattened;
        }
        #endregion
    }
}
