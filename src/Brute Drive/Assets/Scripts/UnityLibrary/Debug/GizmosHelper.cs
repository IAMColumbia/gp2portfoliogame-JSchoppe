using UnityEngine;

namespace UnityLibrary.Debug
{
    #region Color Context Enum
    /// <summary>
    /// Defines the context in which a color is drawn for debug lines.
    /// </summary>
    public enum ColorContext : byte
    {
        /// <summary>
        /// Signals a region in space, typically for a trigger or hull.
        /// </summary>
        Region,
        /// <summary>
        /// Represents the wireframe of a generated mesh or collider.
        /// </summary>
        Wireframe,
        /// <summary>
        /// Represents gizmos indicating a warning.
        /// </summary>
        Warning,
        /// <summary>
        /// Represents gizmos indicating an error.
        /// </summary>
        Error
    }
    #endregion
    /// <summary>
    /// Provides helper methods for gizmo drawing.
    /// </summary>
    public static class GizmosHelper
    {
        #region Parameters
        private const int RING_SEGMENTS = 32;
        private const int RING_RESIZE_SEGMENTS = 8;
        private const float ARROW_HEAD_RATIO = 0.2f;
        #endregion
        #region Draw Shapes
        #region Draw Ring
        /// <summary>
        /// Draws a full ring in the scene view.
        /// </summary>
        /// <param name="location">The world location of the ring center.</param>
        /// <param name="axis">The axis the ring revolves around.</param>
        /// <param name="radius">The radius of the ring.</param>
        public static void DrawRing(Vector3 location, Vector3 axis, float radius)
        {
            // Create the matrix that allows us
            // to convert from a localized z-up solution.
            Matrix4x4 locationAxisMatrix = Matrix4x4.TRS(
                location, Quaternion.LookRotation(axis), Vector3.one * radius);
            // Get the vertices around the ring.
            Vector3[] ringVertices = new Vector3[RING_SEGMENTS];
            for (int i = 0; i < RING_SEGMENTS; i++)
            {
                float angle = (float)i / RING_SEGMENTS * Mathf.PI * 2f;
                ringVertices[i] = locationAxisMatrix.MultiplyPoint(new Vector3(
                    Mathf.Sin(angle), Mathf.Cos(angle), 0f));
            }
            // Draw the segments around the ring.
            for (int i = 1; i < RING_SEGMENTS; i++)
                Gizmos.DrawLine(ringVertices[i - 1], ringVertices[i]);
            Gizmos.DrawLine(ringVertices[RING_SEGMENTS - 1], ringVertices[0]);
        }
        /// <summary>
        /// Draws two full rings in the scene view with arrows indicating resizing.
        /// </summary>
        /// <param name="location">The world location of the ring center.</param>
        /// <param name="axis">The axis the ring revolves around.</param>
        /// <param name="startRadius">The starting radius of the ring.</param>
        /// <param name="endRadius">The ending radius of the ring.</param>
        public static void DrawRingResizing(Vector3 location, Vector3 axis, float startRadius, float endRadius)
        {
            // Create the matrices that allows us
            // to convert from a localized z-up solution.
            Matrix4x4 startRingMatrix = Matrix4x4.TRS(
                location, Quaternion.LookRotation(axis), Vector3.one * startRadius);
            Matrix4x4 endRingMatrix = Matrix4x4.TRS(
                location, Quaternion.LookRotation(axis), Vector3.one * endRadius);
            // Get the vertices around the rings.
            Vector3[] startRingVertices = new Vector3[RING_SEGMENTS];
            Vector3[] endRingVertices = new Vector3[RING_SEGMENTS];
            for (int i = 0; i < RING_SEGMENTS; i++)
            {
                float angle = (float)i / RING_SEGMENTS * Mathf.PI * 2f;
                Vector3 unitPoint = new Vector3(
                    Mathf.Sin(angle), Mathf.Cos(angle), 0f);
                startRingVertices[i] = startRingMatrix.MultiplyPoint(unitPoint);
                endRingVertices[i] = endRingMatrix.MultiplyPoint(unitPoint);
            }
            // Draw the segments around the rings.
            for (int i = 1; i < RING_SEGMENTS; i++)
            {
                Gizmos.DrawLine(startRingVertices[i - 1], startRingVertices[i]);
                Gizmos.DrawLine(endRingVertices[i - 1], endRingVertices[i]);
                // Draw arrows from the start ring to the end ring.
                if ((i - 1) % (RING_SEGMENTS / RING_RESIZE_SEGMENTS) == 0)
                    DrawArrow(startRingVertices[i - 1], endRingVertices[i - 1], axis);
            }
            Gizmos.DrawLine(startRingVertices[RING_SEGMENTS - 1], startRingVertices[0]);
            Gizmos.DrawLine(endRingVertices[RING_SEGMENTS - 1], endRingVertices[0]);
        }
        #endregion
        #region Draw Arrow
        /// <summary>
        /// Draws an arrow with the given start, end, orientation, and arrowhead size.
        /// </summary>
        /// <param name="start">The start of the arrow in world space.</param>
        /// <param name="end">The end of the arrow in world space.</param>
        /// <param name="headForwards">The direction the arrow head faces towards (typically towards the user).</param>
        /// <param name="arrowHeadSize">The size of the arrow head.</param>
        public static void DrawArrow(Vector3 start, Vector3 end, Vector3 headForwards, float arrowHeadSize)
        {
            // Draw the base line.
            Gizmos.DrawLine(start, end);
            // Calculate the arrow head locations.
            Vector3 arrowBase = end + (start - end).normalized * arrowHeadSize;
            Vector3 headOffset = Vector3.Cross(end - start, headForwards).normalized
                * arrowHeadSize * 0.5f;
            Vector3 baseLeft = arrowBase + headOffset;
            Vector3 baseRight = arrowBase - headOffset;
            // Draw the arrow head triangle.
            Gizmos.DrawLine(end, baseLeft);
            Gizmos.DrawLine(end, baseRight);
            Gizmos.DrawLine(baseLeft, baseRight);
        }
        /// <summary>
        /// Draws an arrow with the given start, end, and orientation.
        /// </summary>
        /// <param name="start">The start of the arrow in world space.</param>
        /// <param name="end">The end of the arrow in world space.</param>
        /// <param name="headForwards">The direction the arrow head faces towards (typically towards the user).</param>
        public static void DrawArrow(Vector3 start, Vector3 end, Vector3 headForwards)
        {
            float arrowHeadSize = Vector3.Distance(start, end) * ARROW_HEAD_RATIO;
            DrawArrow(start, end, headForwards, arrowHeadSize);
        }
        /// <summary>
        /// Draws an arrow with the given start, end, and arrow head size.
        /// </summary>
        /// <param name="start">The start of the arrow in world space.</param>
        /// <param name="end">The end of the arrow in world space.</param>
        /// <param name="arrowHeadSize">The size of the arrow head.</param>
        public static void DrawArrow(Vector3 start, Vector3 end, float arrowHeadSize)
        {
            DrawArrow(start, end, Vector3.up, arrowHeadSize);
        }
        /// <summary>
        /// Draws an arrow with the given start and end.
        /// </summary>
        /// <param name="start">The start of the arrow in world space.</param>
        /// <param name="end">The end of the arrow in world space.</param>
        public static void DrawArrow(Vector3 start, Vector3 end)
        {
            DrawArrow(start, end, Vector3.up);
        }
        #endregion
        #endregion
        #region Draw Wireframes
        /// <summary>
        /// Draws a grid given the ordered vertices.
        /// </summary>
        /// <param name="vertices">The verticles that from the grid.</param>
        public static void DrawQuadGrid(Vector3[,] vertices)
        {
            // Draw the lines along each edge loop.
            for (int y = 0; y < vertices.GetLength(1); y++)
                for (int x = 1; x < vertices.GetLength(0); x++)
                    Gizmos.DrawLine(vertices[x, y], vertices[x - 1, y]);
            for (int x = 0; x < vertices.GetLength(0); x++)
                for (int y = 1; y < vertices.GetLength(1); y++)
                    Gizmos.DrawLine(vertices[x, y], vertices[x, y - 1]);
        }
        #endregion
        #region Gizmos Color
        // This setter consolidates color preferences.
        /// <summary>
        /// Sets the gizmo color based on the drawing context.
        /// </summary>
        public static ColorContext Context
        {
            set
            {
                switch (value)
                {
                    case ColorContext.Region:
                        Gizmos.color = Color.cyan; break;
                    case ColorContext.Wireframe:
                        Gizmos.color = Color.green; break;
                    case ColorContext.Warning:
                        Gizmos.color = Color.yellow; break;
                    case ColorContext.Error:
                        Gizmos.color = Color.red; break;
                    default:
                        Gizmos.color = Color.magenta; break;
                }
            }
        }
        #endregion
    }
}
