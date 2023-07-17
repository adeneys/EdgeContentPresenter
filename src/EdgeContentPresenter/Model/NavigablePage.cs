using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    public class NavigablePage
    {
        [JsonPropertyName("id")]
        public string Identifier { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("__typename")]
        public string Type { get; set; }
    }
}
