using UnityEngine;

namespace UnityLibrary.UVConstraints
{
    // TODO this class could be greatly generalized to be more useful.
    /// <summary>
    /// Scrolls the texture UVs to match the orthographic movement
    /// of a transform.
    /// </summary>
    public sealed class TransformUVScroller : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The transform that drives this scrolling effect.")]
        [SerializeField] private Transform drivingTransform = default;
        [Tooltip("The renderer to effect.")]
        [SerializeField] private Renderer targetRenderer = default;
        [Tooltip("How many world units cover one uv unit.")]
        [SerializeField] private float unitsPerUV = 1f;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            unitsPerUV = Mathf.Max(float.Epsilon, unitsPerUV);
        }
        #endregion
        #region UV Update Routine
        private void FixedUpdate()
        {
            // Scale the UV along with transform movement.
            targetRenderer.material.mainTextureOffset =
                new Vector2(
                    -drivingTransform.position.x / unitsPerUV,
                    -drivingTransform.position.z / unitsPerUV);
        }
        #endregion
    }
}
