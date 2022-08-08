using System.Text.Json.Serialization;

namespace EdgeContentPresenter.ContentSource
{
    internal class GraphQLRequest
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("variables")]
        public Dictionary<string, string> Variables { get; set; }
    }
}
