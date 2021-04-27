using GameLibrary.Math;

namespace BruteDriveCore.Objectives
{
    /// <summary>
    /// Implements a waypoint instance.
    /// </summary>
    public interface IWaypoint
    {
        #region Waypoint Properties Implemented
        /// <summary>
        /// The location of the waypoint.
        /// </summary>
        Vector2 Location { get; }
        /// <summary>
        /// The radius of the waypoint.
        /// </summary>
        float Radius { get; set; }
        #endregion
        #region Render Properties Implemented
        /// <summary>
        /// Whether the waypoint should currently be visible.
        /// </summary>
        bool IsRendered { set; }
        /// <summary>
        /// The direction the waypoint should be facing.
        /// </summary>
        Vector2 Direction { get; set; }
        #endregion
    }
}
