using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    internal class SectionTitleContent : Content
    {
        [JsonPropertyName("sectionTitle_Title")]
        public string Title { get; set; }

        public string BackgroundImageUrl { get; set; }
    }
}
