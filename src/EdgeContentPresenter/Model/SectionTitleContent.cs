using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    internal class SectionTitleContent : Content
    {
        [JsonPropertyName("sectionTitle_Title")]
        public string Title { get; set; }

        [JsonPropertyName("sectionTitle_Subtitle")]
        public string Subtitle { get; set; }

        [JsonPropertyName("sectionTitle_Layout")]
        public string Layout { get; set; }

        public string BackgroundImageUrl { get; set; }
    }
}
