namespace EdgeContentPresenter.Settings
{
    public interface IAppSettings
    {
        Task<string> GetEdgeUrlAsync();
        Task SetEdgeUrlAsync(string url);

        Task<string> GetXGqlTokenAsync();
        Task SetXGqlTokenAsync(string token);

        Task<string> GetTitleContentIdentifierAsync();
        Task SetTitleContentIdentifierAsync(string identifier);
    }
}
