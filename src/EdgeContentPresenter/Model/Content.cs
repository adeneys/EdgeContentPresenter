using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    public class Content
    {
        [JsonPropertyName("id")]
        public string Identifier { get; set; }

        public string Type { get; set; }

        public string? NextContentIdentifier { get; set; }

        public string? NextContentType { get; set; }
    }
}
