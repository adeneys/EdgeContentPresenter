using EdgeContentPresenter.Model;

namespace EdgeContentPresenter.ContentSource
{
    internal class DummyContentRepository : IContentRepository
    {
        private List<TextContent> _content = new()
        {
            new TextContent
            {
                Identifier = "page1",
                Type = "Text",
                Title = "Page 1",
                Text = "This is some text one",
                PageHeaderImage = new EdgeImage
                {
                    Uri = new Uri("http://localhost:8090/slide-header.jpg"),
                    Width = 1279,
                    Height = 96
                }
            },
            new TextContent
            {
                Identifier = "page2",
                Type = "Text",
                Title = "Page 2",
                Text = "This is some text two",
                 PageHeaderImage = new EdgeImage
                {
                    Uri = new Uri("http://localhost:8090/slide-header.jpg"),
                    Width = 1279,
                    Height = 96
                }
            },
            new TextContent
            {
                Identifier = "page3",
                Type = "Text",
                Title = "Page 3",
                Text = "This is some text three",
                 PageHeaderImage = new EdgeImage
                {
                    Uri = new Uri("http://localhost:8090/slide-header.jpg"),
                    Width = 1279,
                    Height = 96
                }
            }
        };

        public Task<Content> GetContentAsync(string type, string identifier)
        {
            return Task.FromResult(_content.First(x => x.Identifier == identifier) as Content);
        }

        public Task<IList<NavigablePage>> GetNavigationAsync(string name)
        {
            return Task.FromResult<IList<NavigablePage>>(
                _content.Select(x => new NavigablePage
                {
                    Identifier = x.Identifier,
                    Name = x.Title,
                    Type = "Text"
                }).ToList()
            );
        }
    }
}
