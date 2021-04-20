namespace GameLibrary
{
    /// <summary>
    /// Implemented by editor classes that wrap core classes.
    /// </summary>
    /// <typeparam name="T">The core class or struct being wrapped.</typeparam>
    public interface IEditorWrapper<T>
    {
        #region Methods Implemented
        /// <summary>
        /// Gets the underlying class without editor context.
        /// </summary>
        /// <returns>The wrapped object.</returns>
        T Instance();
        #endregion
    }
}
