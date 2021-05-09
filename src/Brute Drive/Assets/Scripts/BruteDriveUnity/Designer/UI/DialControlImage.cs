using GameLibrary.Input.Touch;
using UnityEngine;
using UnityLibrary.Extensions;
using UnityLibrary.InstanceWrappers.Input.Touch;

namespace BruteDriveUnity.Designer.UI
{
    /// <summary>
    /// A UI image that rotates in response to an input axis.
    /// </summary>
    public sealed class DialControlImage : MonoBehaviour
    {
        #region Inspector Fields
        [Header("References")]
        [Tooltip("The image that is effected.")]
        [SerializeField] private RectTransform imageTransform = default;
        [Tooltip("The circular touch control that drives this image.")]
        [SerializeField] private DialTouchControlInstance circularTouchControl = default;
        #endregion
        #region Initialization
        private void Awake()
        {
            if (circularTouchControl.Instance() is DialHoverTouchControl dialControl)
                dialControl.DialAngleChanged += OnDialAngleChanged;
        }
        private void OnDestroy()
        {
            // Help the GC.
            if (circularTouchControl.Instance() is DialHoverTouchControl dialControl)
                dialControl.DialAngleChanged -= OnDialAngleChanged;
        }
        #endregion
        #region Hover Listener
        private void OnDialAngleChanged(float newAngle)
        {
            // Negative here because canvas forwards is weird in unity.
            imageTransform.SetLocalEulerAngleZ(-newAngle);
        }
        #endregion
    }
}
