using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    internal class BioContent : Content
    {
        [JsonPropertyName("bio_Title")]
        public string Title { get; set; }

        [JsonPropertyName("bio_Name")]
        public string Name { get; set; }

        [JsonPropertyName("bio_JobTitle")]
        public string JobTitle { get; set; }

        [JsonPropertyName("bio_Highlights")]
        public string Highlights { get; set; }

        [JsonPropertyName("bio_Twitter")]
        public string Twitter { get; set; }

        public string ImageUrl { get; set; }
    }
}
