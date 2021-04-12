using GameLibrary;
using UnityEngine;

namespace UnityLibrary.InstanceWrappers
{
    /// <summary>
    /// Base class for behaviours that wrap simpler classes.
    /// </summary>
    /// <typeparam name="T">The object being wrapped by a scene instance.</typeparam>
    public abstract class UnityEditorWrapper<T> : MonoBehaviour, IEditorWrapper<T>
    {
        #region Fields
        private T instance;
        private bool hasInitialized;
        #endregion
        #region MonoBehaviour Stripping
        /// <summary>
        /// Called to retrieve the underlying object.
        /// </summary>
        /// <returns>The simpler class that is wrapped.</returns>
        public T Instance()
        {
            // Initialize the object if needed.
            if (!hasInitialized)
            {
                instance = Initialize();
                hasInitialized = true;
            }
            return instance;
        }
        #endregion
        #region Initialization
        /// <summary>
        /// Initializes the instance to return.
        /// </summary>
        /// <returns>The wrapped instance to expose.</returns>
        public abstract T Initialize();
        #endregion
    }
}
