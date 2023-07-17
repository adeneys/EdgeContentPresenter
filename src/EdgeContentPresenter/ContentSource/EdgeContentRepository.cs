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

        private Dictionary<string, string> _contentTypeQueries = new();

        public EdgeContentRepository(IAppSettings appSettings, IHttpClientFactory httpClientFactory, IContentMapper contentMapper)
        {
            _appSettings = appSettings;
            _httpClientFactory = httpClientFactory;
            _contentMapper = contentMapper;
        }

        public async Task<IList<NavigablePage>> GetNavigationAsync(string name)
        {
            var queryText = await GetContentTypeQuery("Navigation");

            var queryRequest = new GraphQLRequest
            {
                Query = queryText,
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

        public async Task<Content?> GetContentAsync(string type, string identifier)
        {
            var queryText = await GetContentTypeQuery(type);

            var queryRequest = new GraphQLRequest
            {
                Query = queryText,
                Variables = new Dictionary<string, string>
                {
                    { "id", identifier }
                }
            };

            using var response = await PostQuery(queryRequest);

            if (!response.IsSuccessStatusCode)
                throw new EdgeException($"Edge response was not success: {response.StatusCode}");

            var contentJson = await response.Content.ReadAsStringAsync();
            return _contentMapper.MapContentResponse(type, contentJson);
        }

        private async Task<string> GetContentTypeQuery(string type)
        {
            if (_contentTypeQueries.ContainsKey(type))
                return _contentTypeQueries[type];

            using var stream = await FileSystem.OpenAppPackageFileAsync($"ContentSource\\GraphQLQueries\\Get{type}.graphql");
            using var reader = new StreamReader(stream);

            var querytext = reader.ReadToEnd();
            _contentTypeQueries.Add(type, querytext);

            return _contentTypeQueries[type];
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
