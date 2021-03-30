using BruteDrive.Utilities.Unity.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BruteDrive.Designer.Unity
{
    /// <summary>
    /// Utility class for converting static 3D box colliders onto the 2D plane.
    /// </summary>
    public sealed class TopDownColliderConverter : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The colliders to convert.")]
        [SerializeField] private BoxCollider[] boxColliders = default;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            // Create a new empty at the world origin to
            // store the converted colliders.
            GameObject origin = new GameObject("ConvertedColliders");
            foreach (BoxCollider collider in boxColliders)
            {
                GameObject colliderObj = new GameObject();
                colliderObj.transform.parent = origin.transform;
                colliderObj.transform.position = collider.transform.position.TopDownFlatten();
                colliderObj.transform.eulerAngles = Vector3.back * collider.transform.eulerAngles.y;
                colliderObj.transform.localScale = collider.transform.lossyScale.TopDownFlatten();
                
                BoxCollider2D newCollider = colliderObj.AddComponent<BoxCollider2D>();
                newCollider.size = collider.size.TopDownFlatten();
            }
        }
    }
}
