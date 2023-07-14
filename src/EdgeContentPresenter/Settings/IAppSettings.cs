namespace EdgeContentPresenter.Settings
{
    /// <summary>
    /// Defines the settings for the application.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Get the URL of the GraphQL endpoint.
        /// </summary>
        /// <returns>A task encapsulating the URL.</returns>
        Task<string> GetEdgeUrlAsync();

        /// <summary>
        /// Set the URL of the GraphQL endpoint.
        /// </summary>
        /// <param name="url">The URL to set.</param>
        /// <returns>A task encapsulating the operation.</returns>
        Task SetEdgeUrlAsync(string url);

        /// <summary>
        /// Get the X-GQL-Token used to query content from Experience Edge.
        /// </summary>
        /// <returns>A task encapsulating the token.</returns>
        Task<string> GetXGqlTokenAsync();

        /// <summary>
        /// Set the X-GQL-Token used to query content from Experience Edge.
        /// </summary>
        /// <param name="token">The token to set.</param>
        /// <returns>A task encapsulating the operation.</returns>
        Task SetXGqlTokenAsync(string token);

        /// <summary>
        /// Gets the name of the navigation to use.
        /// </summary>
        /// <returns>A task encapsulating the name of the navigation to use.</returns>
        Task<string> GetNavigationNameAsync();

        /// <summary>
        /// Sets the name of the navigation to use.
        /// </summary>
        /// <param name="name">The name to set.</param>
        /// <returns>A task encapsulating the operation.</returns>
        Task SetNavigationNameAsync(string name);
    }
}
