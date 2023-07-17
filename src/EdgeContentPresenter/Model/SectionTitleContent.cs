using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    internal class SectionTitleContent : Content
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("subtitle")]
        public string Subtitle { get; set; }

        public string Layout { get; set; }

        public string BackgroundImageUrl { get; set; }
    }
}
