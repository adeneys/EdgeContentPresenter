﻿using System.Net;
using System.Text;
using System.Text.Json;

namespace EdgeContentPresenter.ContentSource
{
    // Rich text on CHONE is in ProseMirror format. https://prosemirror.net/
    internal class ProseMirrorConverter : IProseMirrorConverter
    {
        private readonly StringBuilder _output = new();
        private int _surpressParagraphTagCount = 0;

        public string ToMauiHtml(string proseMirrorJsonText)
        {
            _output.Clear();
            var doc = JsonDocument.Parse(proseMirrorJsonText);

            ProcessElement(doc.RootElement);

            return _output.ToString();
        }

        private void ProcessElement(JsonElement element)
        {
            // .net MAUI on Windows is very limited in it's tag support: https://github.com/dotnet/maui/blob/main/src/Core/src/Platform/Windows/LabelHtmlHelper.cs
            var type = string.Empty;
            if(element.TryGetProperty("type", out var typeElement))
            {
                type = typeElement.GetString();
            }

            switch(type)
            {
                case "text":
                    var text = element.GetProperty("text").GetString();
                    text = EncodeText(text);
                    _output.Append(text);
                    break;

                case "paragraph":
                    if (_surpressParagraphTagCount == 0) _output.Append("<p>");
                    ProcessChildren(element);
                    if (_surpressParagraphTagCount == 0) _output.Append("</p>");

                    break;

                case "orderedList":
                case "bulletList":
                    _surpressParagraphTagCount++;
                    _output.Append("<ul>");
                    ProcessChildren(element);
                    _output.Append("</ul>");
                    _surpressParagraphTagCount--;
                    break;

                case "listItem":
                    _output.Append("<li>");
                    ProcessChildren(element);
                    _output.Append("</li>");
                    break;

                default:
                    ProcessChildren(element);
                    break;

            }
        }

        private void ProcessChildren(JsonElement element)
        {
            if (element.TryGetProperty("content", out JsonElement contentElement))
            {
                foreach (var child in contentElement.EnumerateArray())
                {
                    ProcessElement(child);
                }
            }
        }

        private string EncodeText(string text)
        {
            return WebUtility.HtmlEncode(text);
        }
    }
}
