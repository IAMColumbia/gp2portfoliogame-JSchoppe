using UnityEngine;
using UnityEngine.UI;
using UnityLibrary.InstanceWrappers.Input.Touch;
using GameLibrary.Input.Touch;

namespace BruteDriveUnity.Designer.UI
{
    /// <summary>
    /// A UI image that responds to change in button input state.
    /// </summary>
    public sealed class TouchControlToggleImage : MonoBehaviour
    {
        #region Inspector Fields
        [Header("References")]
        [Tooltip("The image that is effected.")]
        [SerializeField] private Image image = default;
        [Tooltip("The texture displayed above the threshold.")]
        [SerializeField] private Sprite aboveThresholdTexture = default;
        [Tooltip("The texture displayed under the threshold.")]
        [SerializeField] private Sprite belowThresholdTexture = default;
        [Tooltip("The touch control that drives this image.")]
        [SerializeField] private TouchControlInstance drivingInstance = default;
        #endregion
        #region Initialization
        private void Awake()
        {
            // Set the initial image based on inspector parameter.
            if (drivingInstance.Instance() is HoverTouchControl touchControl)
                touchControl.HoverStateChanged += OnHoverStateChanged;
        }
        private void OnDestroy()
        {
            // Assist GC in cleanup.
            if (drivingInstance.Instance() is HoverTouchControl touchControl)
                touchControl.HoverStateChanged -= OnHoverStateChanged;
        }
        #endregion
        #region Hover Listener
        private void OnHoverStateChanged(bool isHovered)
        {
            image.sprite = isHovered ?
                aboveThresholdTexture : belowThresholdTexture;
        }
        #endregion
    }
}
