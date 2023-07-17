using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    internal class TextContent : Content
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        public string Text { get; set; }

        public string PageHeaderImageUrl { get; set; }

        public IReadOnlyList<string> ImageUrls { get; set; }
    }
}
