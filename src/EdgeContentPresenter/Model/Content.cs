﻿using System.Text.Json.Serialization;

namespace EdgeContentPresenter.Model
{
    public class Content
    {
        [JsonPropertyName("id")]
        public string Identifier { get; set; }

        public string Type { get; set; }
    }
}
