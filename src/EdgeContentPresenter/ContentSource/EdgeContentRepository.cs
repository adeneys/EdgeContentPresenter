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
        
        private string? _query = null;

        public EdgeContentRepository(IAppSettings appSettings, IHttpClientFactory httpClientFactory, IContentMapper contentMapper)
        {
            _appSettings = appSettings;
            _httpClientFactory = httpClientFactory;
            _contentMapper = contentMapper;
        }

        public async Task<Content?> GetContentAsync(string identifier)
        {
            await EnsureQueryTextLoaded();
            var xGqlToken = await _appSettings.GetXGqlTokenAsync();

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-GQL-Token", xGqlToken);

            var edgeUrl = await _appSettings.GetEdgeUrlAsync();
            var baseUri = new Uri(edgeUrl);
            var uri = new Uri(baseUri, "/api/graphql/v1");
            var queryRequest = new GraphQLRequest
            {
                Query = _query,
                Variables = new Dictionary<string, string>
                {
                    { "id", identifier }
                }
            };

            // todo: cancellation token
            using var response = await httpClient.PostAsJsonAsync(uri, queryRequest);

            if (!response.IsSuccessStatusCode)
                throw new EdgeException($"Edge response was not success: {response.StatusCode}");

            var contentJson = await response.Content.ReadAsStringAsync();
            return _contentMapper.MapContentResponse(contentJson);
        }

        private async Task EnsureQueryTextLoaded()
        {
            if (_query != null)
                return;

            using var stream = await FileSystem.OpenAppPackageFileAsync("ContentSource\\GraphQLQueries\\GetContent.graphql");
            using var reader = new StreamReader(stream);

            _query = reader.ReadToEnd();
        }
    }
}
