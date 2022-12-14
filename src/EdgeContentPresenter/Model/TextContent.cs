using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    internal class TextContent : Content
    {
        [JsonPropertyName("text_Title")]
        public string Title { get; set; }

        [JsonPropertyName("text_Text")]
        public string Text { get; set; }

        public string PageHeaderImageUrl { get; set; }

        public string MainImageUrl { get; set; }
    }
}
