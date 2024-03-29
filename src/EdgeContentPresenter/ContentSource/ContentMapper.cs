﻿using EdgeContentPresenter.Model;
using System.Text.Json;

namespace EdgeContentPresenter.ContentSource
{
    internal class ContentMapper : IContentMapper
    {
        private const string HeaderAssetTypeId = "PresentationAssets.PageHeader";

        public IList<NavigablePage> MapNavigationResponse(string content)
        {
            var json = JsonDocument.Parse(content);
            var pages = new List<NavigablePage>();

            try
            {
                var navigationResults = json.RootElement
                    .GetProperty("data")
                    .GetProperty("allNavigation")
                    .GetProperty("results")
                    .EnumerateArray();

                if (!navigationResults.MoveNext())
                {
                    throw new EdgeException("Failed to find navigation");
                }

                var pageResults = navigationResults.Current
                    .GetProperty("pages")
                    .GetProperty("results")
                    .EnumerateArray();

                if (pageResults.Any())
                {
                    foreach (var pageResult in pageResults)
                    {
                        var page = pageResult.Deserialize<NavigablePage>();
                        if(page != null)
                            pages.Add(page);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new EdgeException("Error deserializing navigation JSON", ex);
            }

            return pages;
        }

        public Content? MapContentResponse(string type, string content)
        {
            var json = JsonDocument.Parse(content);

            try
            {
                var data = json.RootElement.GetProperty("data");
                return Deserialize(type, data);
            }
            catch(Exception ex)
            {
                throw new EdgeException("Error deserializing content JSON", ex);
            }
        }

        private Content? Deserialize(string type, JsonElement element)
        {
            return type switch
            {
                "Title" => DeserializeTitleContent(element, type),
                "Bio" => DeserializeBioContent(element, type),
                "Text" => DeserializeTextContent(element, type),
                "SectionTitle" => DeserializeSectionTitleContent(element, type),
                "Social" => DeserializeSocialContent(element, type),
                _ => throw new EdgeException("Unknown content type " + type)
            };
        }

        private T? Deserialize<T>(JsonElement element, string type)
            where T : Content
        {
            var result = element.Deserialize<T>();
            result.Type = type;
            return result;
        }

        private Content? DeserializeTitleContent(JsonElement element, string type)
        {
            var contentElement = element.GetProperty("title");
            var result = Deserialize<TitleContent>(contentElement, type);
            result.BackgroundImage = ResolveImages(contentElement, "backgroundImage").FirstOrDefault();
            
            // Date is stored in CHONE at UTC. Need to convert it to local.
            result.Date = result.Date.ToLocalTime();

            var hashtags = result.HashTags.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if(hashtags.Any())
                result.HashTags = string.Join("   ", hashtags.Select(x => "#" + x));

            return result;
        }

        private Content? DeserializeBioContent(JsonElement element, string type)
        {
            var contentElement = element.GetProperty("bio");
            var result = Deserialize<BioContent>(contentElement, type);
            result.Highlights = ResolveRichText(contentElement, "highlights");
            result.Image = ResolveImages(contentElement, "profileImage").FirstOrDefault();
            result.PageHeaderImage = ResolveImages(contentElement, "headerImage").FirstOrDefault();
            return result;
        }

        private Content? DeserializeTextContent(JsonElement element, string type)
        {
            var contentElement = element.GetProperty("text");
            var result = Deserialize<TextContent>(contentElement, type);
            result.Text = ResolveRichText(contentElement, "text");
            result.PageHeaderImage = ResolveImages(contentElement, "headerImage").FirstOrDefault();
            result.Images = ResolveImages(contentElement, "images").AsReadOnly();
            return result;
        }

        private Content? DeserializeSectionTitleContent(JsonElement element, string type)
        {
            var contentElement = element.GetProperty("sectionTitle");
            var result = Deserialize<SectionTitleContent>(contentElement, type);
            result.BackgroundImage = ResolveImages(contentElement, "backgroundImage").FirstOrDefault();
            result.Layout = ResolveLayout(contentElement, "layout");
            return result;
        }

        private Content? DeserializeSocialContent(JsonElement element, string type)
        {
            var contentElement = element.GetProperty("social");
            var result = Deserialize<SocialContent>(contentElement, type);
            result.BackgroundImage = ResolveImages(contentElement, "backgroundImage").FirstOrDefault();

            return result;
        }

        private IList<EdgeImage> ResolveImages(JsonElement element, string relationName)
        {
            var images = new List<EdgeImage>();

            var mediaList = element
                .GetProperty(relationName)
                .GetProperty("results")
                .EnumerateArray();

            if (mediaList.Any())
            {
                foreach (var media in mediaList)
                {
                    var rawUri = ResolveStringProperty(media, "fileUrl");
                    if(rawUri != null)
                    {
                        if(Uri.TryCreate(rawUri, UriKind.Absolute, out var uri))
                        {
                            var width = ResolveIntProperty(media, "fileWidth");
                            var height = ResolveIntProperty(media, "fileHeight");

                            var image = new EdgeImage
                            {
                                Uri = uri,
                                Width = width,
                                Height = height
                            };

                            images.Add(image);
                        }
                    }
                }
            }

            return images;
        }

        private string? ResolveStringProperty(JsonElement element, string name)
        {
            var prop = element.GetProperty(name);
            if (prop.ValueKind != JsonValueKind.Undefined)
                return prop.GetString();

            return null;
        }

        private int ResolveIntProperty(JsonElement element, string name)
        {
            var prop = element.GetProperty(name);
            if (prop.ValueKind != JsonValueKind.Undefined)
                return prop.GetInt32();

            return 0;
        }

        private string? ResolveRichText(JsonElement element, string fieldName)
        {
            // Rich text on CH-ONE is in ProseMirror format. https://prosemirror.net/
            var data = element.GetProperty(fieldName);
            if (data.ValueKind != JsonValueKind.Undefined && data.ValueKind != JsonValueKind.Null)
            {
                var json = data.ToString();
                var conv = new ProseMirrorConverter();
                var html = conv.ToMauiHtml(json);

                return html;
            }

            return null;
        }

        private string? ResolveLayout(JsonElement element, string fieldName)
        {
            var results = element
                .GetProperty(fieldName)
                .GetProperty("results")
                .EnumerateArray();

            if(results.Any() && results.MoveNext())
            {
                return results.Current.GetProperty("id").GetString();
            }

            return null;
        }
    }
}
