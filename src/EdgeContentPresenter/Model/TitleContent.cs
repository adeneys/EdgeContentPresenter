using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    public class TitleContent : Content
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("hashtags")]
        public string HashTags { get; set; }

        public EdgeImage BackgroundImage { get; set; }
    }
}
