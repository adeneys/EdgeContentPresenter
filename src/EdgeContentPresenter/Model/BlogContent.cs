using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    internal class BlogContent : Content
    {
        [JsonPropertyName("blog_Title")]
        public string Title { get; set; }

        [JsonPropertyName("blog_Quote")]
        public string Quote { get; set; }

        [JsonPropertyName("blog_Body")]
        public string Body { get; set; }
    }
}
