using GameLibrary.Math;

namespace GameLibrary.Transforms
{
    // TODO research better ways to project implementation
    // requirements onto existing classes.
    /// <summary>
    /// Implements a transform that can be updated
    /// to look in a given direction from a given position.
    /// </summary>
    public interface IOrientedTransform3D
    {
        #region Properties Implemented
        /// <summary>
        /// The position of the object.
        /// </summary>
        Vector3 Position { get; set; }
        /// <summary>
        /// The forwards direction of the object.
        /// </summary>
        Vector3 Forwards { get; set; }
        #endregion
    }
}
