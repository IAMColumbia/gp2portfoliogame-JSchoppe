using UnityEngine;

namespace UnityLibrary.TopDown2D
{
    // TODO this class only handles cube->box colliders, could
    // be further generalized to project more collider shapes.
    /// <summary>
    /// Utility class for converting static 3D box colliders onto the 2D plane.
    /// </summary>
    public sealed class TopDownColliderConverter : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The colliders to convert.")]
        [SerializeField] private BoxCollider[] boxColliders = default;
        #endregion
        #region Collider Converter
        private void Awake()
        {
            // Create a new empty at the world origin to
            // store the converted colliders.
            GameObject origin = new GameObject("ConvertedColliders");
            foreach (BoxCollider collider in boxColliders)
            {
                GameObject colliderObj = new GameObject();
                // Convert the collider into 2D space.
                colliderObj.transform.parent = origin.transform;
                colliderObj.transform.position = collider.transform.position.TopDownFlatten();
                colliderObj.transform.eulerAngles = Vector3.back * collider.transform.eulerAngles.y;
                colliderObj.transform.localScale = collider.transform.lossyScale.TopDownFlatten();
                // Add the box collider by flattening the 3D collider.
                BoxCollider2D newCollider = colliderObj.AddComponent<BoxCollider2D>();
                newCollider.size = collider.size.TopDownFlatten();
            }
            // Remove this object from the hierarchy.
            Destroy(this);
        }
        #endregion
    }
}
