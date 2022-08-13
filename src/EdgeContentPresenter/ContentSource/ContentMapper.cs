using EdgeContentPresenter.Model;
using System.Text.Json;

namespace EdgeContentPresenter.ContentSource
{
    internal class ContentMapper : IContentMapper
    {
        private const string HeaderAssetTypeId = "PresentationAssets.PageHeader";

        public Content? MapContentResponse(string content)
        {
            var json = JsonDocument.Parse(content);

            try
            {
                var result = json.RootElement
                    .GetProperty("data")
                    .GetProperty("m_Content");

                var type = result
                    .GetProperty("contentTypeToContent")
                    .GetProperty("id")
                    .GetString();

                return Deserialize(result, type);
            }
            catch(Exception ex)
            {
                throw new EdgeException("Error deserializing content JSON", ex);
            }
        }

        private Content? Deserialize(JsonElement element, string type)
        {
            return type switch
            {
                "M.ContentType.Blog" => Deserialize<BlogContent>(element, type),
                "M.ContentType.Title" => DeserializeTitleContent(element, type),
                "M.ContentType.Bio" => DeserializeBioContent(element, type),
                "M.ContentType.SectionTitle" => DeserializeSectionTitleContent(element, type),
                "M.ContentType.Text" => DeserializeTextContent(element, type),
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
            var result = Deserialize<TitleContent>(element, type);
            result.BackgroundImageUrl = ResolvePrimaryImageUrl(element);
            result.NextContentIdentifier = ResolveNextContentIdentifier(element, "reference_Title_Next_Parents");
            return result;
        }

        private Content? DeserializeBioContent(JsonElement element, string type)
        {
            var result = Deserialize<BioContent>(element, type);
            result.ImageUrl = ResolvePrimaryImageUrl(element);
            result.PageHeaderImageUrl = ResolveImageUrl(element, "pageHeader", null, null);
            result.NextContentIdentifier = ResolveNextContentIdentifier(element, "reference_Bio_Next_Parents");
            return result;
        }

        private Content? DeserializeSectionTitleContent(JsonElement element, string type)
        {
            var result = Deserialize<SectionTitleContent>(element, type);
            result.BackgroundImageUrl = ResolvePrimaryImageUrl(element);
            result.NextContentIdentifier = ResolveNextContentIdentifier(element, "reference_SectionTitle_Next_Parents");
            return result;
        }

        private Content? DeserializeTextContent(JsonElement element, string type)
        {
            var result = Deserialize<TextContent>(element, type);
            result.PageHeaderImageUrl = ResolveImageUrl(element, "pageHeader", null, null);
            result.MainImageUrl = ResolvePrimaryImageUrl(element);
            result.NextContentIdentifier = ResolveNextContentIdentifier(element, "reference_Text_Next_Parents");
            return result;
        }

        private string? ResolvePrimaryImageUrl(JsonElement element)
        {
            var url = ResolveImageUrl(element, "cmpContentToMasterLinkedAsset", null, null);

            if(string.IsNullOrEmpty(url))
                url = ResolveImageUrl(element, "cmpContentToLinkedAsset", null, HeaderAssetTypeId);

            return url;
        }

        private string? ResolveImageUrl(JsonElement element, string relationName, string includePresentationAsset, string excludePresentationAsset)
        {
            var assetList = element
                .GetProperty(relationName)
                .GetProperty("results")
                .EnumerateArray();

            if (assetList.Any())
            {
                // Find first asset that satisfies the include & exclude parameters
                foreach (var asset in assetList)
                {
                    var types = ResolveAssetPresentationType(asset);

                    if(!string.IsNullOrEmpty(includePresentationAsset))
                    {
                        if (types.All(x => x != includePresentationAsset))
                            continue;
                    }

                    if (!string.IsNullOrEmpty(excludePresentationAsset))
                    {
                        if (types.Any(x => x == excludePresentationAsset))
                            continue;
                    }

                    return ResolveImageUrl(asset);
                }
            }

            return null;
        }

        private IList<string> ResolveAssetPresentationType(JsonElement element)
        {
            var types = new List<string>();

            if(!element.TryGetProperty("presentationAssets", out var presentationAssetsElement))
                return types;

            var presentationAssets = presentationAssetsElement
                .GetProperty("results")
                .EnumerateArray();

            foreach (var type in presentationAssets)
            {
                var presentationAssetId = type
                    .GetProperty("id")
                    .GetString();

                if (!string.IsNullOrEmpty(presentationAssetId))
                    types.Add(presentationAssetId);
            }

            return types;
        }

        private string? ResolveImageUrl(JsonElement element)
        {
            var urls = element.GetProperty("urls");
            var urlsEnumerator = urls.EnumerateObject();

            var firstUrl = urlsEnumerator.FirstOrDefault();
            if(firstUrl.Value.ValueKind != JsonValueKind.Undefined)
                return firstUrl.Value.GetProperty("url").GetString();

            return null;
        }

        private string? ResolveNextContentIdentifier(JsonElement element, string relationName)
        {
            if (!element.TryGetProperty(relationName, out var relation))
                return null;

            var contentList = relation
                .GetProperty("results")
                .EnumerateArray();

            var firstContent = contentList.FirstOrDefault();

            if (firstContent.ValueKind != JsonValueKind.Undefined)
                return firstContent
                    .GetProperty("id")
                    .GetString();

            return null;
        }
    }
}
