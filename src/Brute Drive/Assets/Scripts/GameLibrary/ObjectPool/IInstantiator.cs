namespace GameLibrary.ObjectPool
{
    /// <summary>
    /// Implements an instantiator for an object type.
    /// </summary>
    /// <typeparam name="T">The object that will be instantiated.</typeparam>
    public interface IInstantiator<T>
    {
        #region Methods Implemented
        /// <summary>
        /// Makes an instantiated copy of the object.
        /// </summary>
        /// <returns>A new object instance.</returns>
        public T Instantiate();
        #endregion
    }
}
