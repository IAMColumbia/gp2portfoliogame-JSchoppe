using GameLibrary.Input.Touch;
using UnityEngine;

namespace UnityLibrary.InstanceWrappers.Input.Touch
{
    /// <summary>
    /// Base class for all touch control instances.
    /// </summary>
    public abstract class TouchControlInstance : UnityEditorWrapper<TouchControl>
    {
        #region Inspector Fields
        [Tooltip("The canvas that this control is on.")]
        [SerializeField] protected Canvas canvas = default;
        #endregion
        #region Layout Changed Listener
        /// <summary>
        /// Called when the Unity screen size changes.
        /// Recalculates the positioning of the control.
        /// </summary>
        public void OnLayoutChanged()
        {
            Instance().OriginLocation = (Vector2)transform.position;
        }
        #endregion
    }
}
