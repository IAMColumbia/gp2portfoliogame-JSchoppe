using UnityEngine;
using UnityEngine.InputSystem;
using BruteDrive.Utilities.Unity.Extensions;

namespace BruteDrive.Designer.Unity.UI
{
    /// <summary>
    /// A UI image that rotates in response to an input axis.
    /// </summary>
    public sealed class RotationDrivenImage : MonoBehaviour
    {
        #region Inspector Fields
        [Header("References")]
        [Tooltip("The image that is effected.")]
        [SerializeField] private RectTransform imageTransform = default;
        [Header("Input Parameters")]
        [Tooltip("Input value will be multiplied by this unit in degrees.")]
        [SerializeField] private float degreesFactor = 90f;
        #endregion
        #region Input Listeners
        // Listens to the new input system and
        // updates the rotation.
        public void RecieveAxis(InputAction.CallbackContext context)
        {
            imageTransform.SetLocalEulerAngleZ(
                context.ReadValue<float>() * degreesFactor);
        }
        #endregion
    }
}
