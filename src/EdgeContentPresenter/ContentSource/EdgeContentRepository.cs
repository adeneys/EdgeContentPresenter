using EdgeContentPresenter.Model;
using EdgeContentPresenter.Settings;
using System.Net.Http.Json;

namespace EdgeContentPresenter.ContentSource
{
    internal class EdgeContentRepository : IContentRepository
    {
        private readonly IAppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IContentMapper _contentMapper;
        
        private string? _contentQquery = null;
        private string? _navigationQuery = null;

        public EdgeContentRepository(IAppSettings appSettings, IHttpClientFactory httpClientFactory, IContentMapper contentMapper)
        {
            _appSettings = appSettings;
            _httpClientFactory = httpClientFactory;
            _contentMapper = contentMapper;
        }

        public async Task<IList<NavigablePage>> GetNavigationAsync(string name)
        {
            await EnsureQueryTextLoaded();

            var queryRequest = new GraphQLRequest
            {
                Query = _navigationQuery,
                Variables = new Dictionary<string, string>
                {
                    { "name", name }
                }
            };

            using var response = await PostQuery(queryRequest);

            if (!response.IsSuccessStatusCode)
                throw new EdgeException($"Edge response was not success: {response.StatusCode}");

            var contentJson = await response.Content.ReadAsStringAsync();
            return _contentMapper.MapNavigationResponse(contentJson);
        }

        public async Task<Content?> GetContentAsync(string identifier)
        {
            await EnsureQueryTextLoaded();
            
            var queryRequest = new GraphQLRequest
            {
                Query = _contentQquery,
                Variables = new Dictionary<string, string>
                {
                    { "id", identifier }
                }
            };

            using var response = await PostQuery(queryRequest);

            if (!response.IsSuccessStatusCode)
                throw new EdgeException($"Edge response was not success: {response.StatusCode}");

            var contentJson = await response.Content.ReadAsStringAsync();
            return _contentMapper.MapContentResponse(contentJson);
        }

        private async Task EnsureQueryTextLoaded()
        {
            if (_contentQquery == null)
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("ContentSource\\GraphQLQueries\\GetContent.graphql");
                using var reader = new StreamReader(stream);

                _contentQquery = reader.ReadToEnd();
            }

            if (_navigationQuery == null)
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("ContentSource\\GraphQLQueries\\GetNavigation.graphql");
                using var reader = new StreamReader(stream);

                _navigationQuery = reader.ReadToEnd();
            }
        }

        private async Task<HttpResponseMessage> PostQuery(GraphQLRequest queryRequest)
        {
            var xGqlToken = await _appSettings.GetXGqlTokenAsync();

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-GQL-Token", xGqlToken);

            var edgeUrl = await _appSettings.GetEdgeUrlAsync();
            var baseUri = new Uri(edgeUrl);
            var uri = new Uri(baseUri, "/api/graphql/v1");

            // todo: cancellation token
            return await httpClient.PostAsJsonAsync(uri, queryRequest);
        }
    }
}
