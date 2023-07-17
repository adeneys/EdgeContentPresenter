using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    internal class BioContent : Content
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("jobTitle")]
        public string JobTitle { get; set; }

        public string Highlights { get; set; }

        [JsonPropertyName("mastodon")]
        public string MastodonUrl { get; set; }

        public string PageHeaderImageUrl { get; set; }

        public string ImageUrl { get; set; }
    }
}
