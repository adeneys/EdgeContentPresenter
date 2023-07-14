namespace EdgeContentPresenter.Settings
{
    internal class AppSettings : IAppSettings
    {
        private const string EdgeUrlSettingName = "EdgeUrl";
        private const string XGqlTokenSettingName = "XGQLToken";
        private const string NavigationNameSettingName = "NavigationName";

        private const string DefaultEdgeUrl = "https://edge.sitecorecloud.io";
        private const string DefaultNavigationName = "Primary Navigation";

        private readonly ISecureStorage _secureStorage;

        public AppSettings()
        {
            _secureStorage = SecureStorage.Default;
        }

        public async Task<string> GetEdgeUrlAsync()
        {
            return await _secureStorage.GetAsync(EdgeUrlSettingName) ?? DefaultEdgeUrl;
        }

        public async Task SetEdgeUrlAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                _secureStorage.Remove(EdgeUrlSettingName);
            else
                await _secureStorage.SetAsync(EdgeUrlSettingName, url);
        }

        public async Task<string> GetXGqlTokenAsync()
        {
            return await _secureStorage.GetAsync(XGqlTokenSettingName);
        }

        public async Task SetXGqlTokenAsync(string token)
{
            if (string.IsNullOrEmpty(token))
                _secureStorage.Remove(XGqlTokenSettingName);
            else
                await _secureStorage.SetAsync(XGqlTokenSettingName, token);
        }

        public async Task<string> GetNavigationNameAsync()
        {
            return await _secureStorage.GetAsync(NavigationNameSettingName) ?? DefaultNavigationName;
        }

        public async Task SetNavigationNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                _secureStorage.Remove(NavigationNameSettingName);
            else
                await _secureStorage.SetAsync(NavigationNameSettingName, name);
        }
    }
}
