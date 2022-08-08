using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    public class TitleContent : Content
    {
        [JsonPropertyName("title_Title")]
        public string Title { get; set; }

        [JsonPropertyName("title_Author")]
        public string Author { get; set; }

        [JsonPropertyName("title_Date")]
        public string Date { get; set; }

        [JsonPropertyName("title_HashTags")]
        public string HashTags { get; set; }

        public string BackgroundImageUrl { get; set; }
    }
}
