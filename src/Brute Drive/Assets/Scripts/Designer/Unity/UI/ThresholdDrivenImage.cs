using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace BruteDrive.Designer.Unity.UI
{
    /// <summary>
    /// A UI image that responds to change in button input state.
    /// </summary>
    public sealed class ThresholdDrivenImage : MonoBehaviour
    {
        #region Inspector Fields
        [Header("References")]
        [Tooltip("The image that is effected.")]
        [SerializeField] private Image image = default;
        [Tooltip("The texture displayed above the threshold.")]
        [SerializeField] private Sprite aboveThresholdTexture = default;
        [Tooltip("The texture displayed under the threshold.")]
        [SerializeField] private Sprite belowThresholdTexture = default;
        [Header("Threshold Parameters")]
        [Tooltip("The state of the driving input.")]
        [SerializeField] private bool isPressed = false;
        [Tooltip("The input threshold where the image changes.")]
        [Range(-1f, 1f)][SerializeField] private float threshold = 0.5f;
        #endregion
        #region Initialization
        private void Awake()
        {
            // Set the initial image based on inspector parameter.
            image.sprite = isPressed ?
                aboveThresholdTexture : belowThresholdTexture;
        }
        #endregion
        #region Input Listeners
        // Listens to the new input system and
        // updates the texture.
        public void RecieveAxis(InputAction.CallbackContext context)
        {
            isPressed = context.ReadValue<float>() > threshold;
            image.sprite = isPressed ?
                aboveThresholdTexture : belowThresholdTexture;
        }
        #endregion
    }
}
