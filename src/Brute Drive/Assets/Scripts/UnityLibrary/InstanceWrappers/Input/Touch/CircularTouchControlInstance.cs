using GameLibrary.Input.Touch;
using GameLibrary.Input.Touch.Shapes;
using UnityEngine;
using UnityLibrary.Debug;

namespace UnityLibrary.InstanceWrappers.Input.Touch
{
    /// <summary>
    /// Wraps a scene instance of a circular touch control.
    /// </summary>
    public sealed class CircularTouchControlInstance : TouchControlInstance
    {
        #region Inspector Fields
        [Tooltip("The pixel radius of the base circle.")]
        [SerializeField] private float pixelsRadius = 10f;
        [Tooltip("The expanded radius once hover starts.")]
        [SerializeField] private float hoveredRadius = 15f;
        #endregion
#if UNITY_EDITOR
        #region Inspector Validation
        private void OnValidate()
        {
            hoveredRadius = Mathf.Max(hoveredRadius, pixelsRadius);
        }
        #endregion
        #region Gizmos Drawing
        private void OnDrawGizmosSelected()
        {
            // Do null checks to avoid errors on gizmo drawing.
            if (canvas != null)
            {
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                if (canvasRect != null)
                {
                    // Draw the rings to assist designers.
                    GizmosHelper.Context = ColorContext.Region;
                    GizmosHelper.DrawRingResizing(
                        transform.position, transform.forward, pixelsRadius, hoveredRadius);
                }
            }
        }
        #endregion
#endif
        #region Wrapper Accessor
        /// <summary>
        /// Initializes the wrapped touch control.
        /// </summary>
        /// <returns>The touch control class held in this Unity instance.</returns>
        public override TouchControl Initialize()
            => new ExpandingHoverTouchControl(new CircleRegion2D(pixelsRadius))
            {
                IsEnabled = true,
                HoverExpansion = hoveredRadius - pixelsRadius,
                OriginLocation = (Vector2)transform.position
            };
        #endregion
    }
}
