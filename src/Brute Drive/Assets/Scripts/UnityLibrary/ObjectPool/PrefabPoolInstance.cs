using GameLibrary.ObjectPool;
using UnityEngine;
using UnityLibrary.InstanceWrappers;

namespace UnityLibrary.ObjectPool
{
    // TODO the code architecture here exposes Instantiate,
    // maybe hide that by passing the instantiator function
    // as a delegate instead of by interface.
    /// <summary>
    /// Implements an object pool from a prefab template.
    /// </summary>
    public sealed class PrefabPoolInstance : UnityEditorWrapper<ObjectPool<GameObject>>, IInstantiator<GameObject>
    {
        #region Inspector Fields
        [Tooltip("The template used for this object pool.")]
        [SerializeField] private GameObject prefab = default;
        [Tooltip("The number of instances to preload.")]
        [SerializeField] private int startingCount = 0;
        [Tooltip("When toggled new instances are placed under this pool transform.")]
        [SerializeField] private bool instancesUnderThisTransform = true;
        #endregion
        #region Inspector Validation
        private void OnValidate()
        {
            startingCount = Mathf.Max(0, startingCount);
        }
        #endregion
        #region IInstantiation Procedure
        public GameObject Instantiate()
        {
            GameObject newInstance = Instantiate(prefab);
            if (instancesUnderThisTransform)
                newInstance.transform.parent = transform;
            return newInstance;
        }
        #endregion
        #region Wrapper Accessor
        public override ObjectPool<GameObject> Initialize()
        {
            return new ObjectPool<GameObject>(this, startingCount);
        }
        #endregion
    }
}
