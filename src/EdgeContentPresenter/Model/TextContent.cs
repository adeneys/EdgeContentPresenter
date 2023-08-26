using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    internal class TextContent : Content
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        public string Text { get; set; }

        public EdgeImage? PageHeaderImage { get; set; }

        public Color PageHeaderFillColor { get; set; } = Color.Parse("DarkGray");

        public IReadOnlyList<EdgeImage> Images { get; set; }

        public int MaximumImageWidth => 1200 / Images.Count;
    }
}
