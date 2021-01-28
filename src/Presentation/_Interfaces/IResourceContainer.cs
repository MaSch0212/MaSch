namespace MaSch.Presentation
{
    /// <summary>
    /// Providers methods to find a resource in some kind of resource container.
    /// </summary>
    public interface IResourceContainer
    {
        /// <summary>
        /// Finds the resource with a given key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>The resource with the given key.</returns>
        object FindResource(object resourceKey);

        /// <summary>
        /// Tries the find the resource with a given key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>The resource with the given key.</returns>
        object TryFindResource(object resourceKey);
    }
}
