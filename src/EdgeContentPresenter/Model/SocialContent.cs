using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    public class SocialContent : Content
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("slack")]
        public string Slack { get; set; }

        [JsonPropertyName("mastodon")]
        public string Mastodon { get; set; }

        [JsonPropertyName("stackExchange")]
        public string StackExchange { get; set; }

        public EdgeImage BackgroundImage { get; set; }
    }
}
