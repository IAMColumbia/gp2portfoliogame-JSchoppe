using System.Collections.Generic;

namespace GameLibrary.ObjectPool
{
    /// <summary>
    /// Defines the logical structure for an object pool.
    /// </summary>
    /// <typeparam name="T">The type of object in the pool.</typeparam>
    public sealed class ObjectPool<T>
    {
        #region Fields
        private readonly IInstantiator<T> instantiator;
        private readonly Stack<T> freeInstances;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new object pool with the given instantiator and starting count.
        /// </summary>
        /// <param name="instantiator">The object containing the instantiation routine.</param>
        /// <param name="startingCount">The number of items to preload.</param>
        public ObjectPool(IInstantiator<T> instantiator, int startingCount)
        {
            this.instantiator = instantiator;
            // Preload some instances into the pool.
            freeInstances = new Stack<T>();
            for (int i = 0; i < startingCount; i++)
                freeInstances.Push(instantiator.Instantiate());
        }
        /// <summary>
        /// Creates a new object pool with the given instantiator.
        /// </summary>
        /// <param name="instantiator">The object containing the instantiation routine.</param>
        public ObjectPool(IInstantiator<T> instantiator) : this(instantiator, 0)
        {

        }
        #endregion
        #region Pool Methods
        /// <summary>
        /// Returns a free instance.
        /// </summary>
        /// <returns>
        /// The next free instance, if none are available
        /// a new instance is created.
        /// </returns>
        public T RetrieveInstance()
        {
            // If the pool is exhuasted, add a new element to it.
            if (freeInstances.Count == 0)
                freeInstances.Push(instantiator.Instantiate());
            // Return a free instance to activate.
            return freeInstances.Pop();
        }
        /// <summary>
        /// Marks an object to be recycled next time an instance is requested.
        /// This can also be used to merge external items into the pool.
        /// </summary>
        /// <param name="instance">The instance to retire.</param>
        public void RetireInstance(T instance)
        {
            // Prevent duplicate items in the pool.
            if (!freeInstances.Contains(instance))
                freeInstances.Push(instance);
        }
        /// <summary>
        /// Disassociates all objects from this pool.
        /// </summary>
        public void FlushPool()
        {
            // Clear out all free instances so they
            // can no longer be pulled from the pool.
            freeInstances.Clear();
        }
        #endregion
    }
}
