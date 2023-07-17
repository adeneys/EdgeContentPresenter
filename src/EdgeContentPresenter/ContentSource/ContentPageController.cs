using EdgeContentPresenter.ContentTypes;
using EdgeContentPresenter.Model;
using System.ComponentModel;

namespace EdgeContentPresenter.ContentSource
{
    internal class ContentPageController : IContentPageController
    {
        private readonly IContentRepository _contentRepository;
        private readonly IContentPageFactory _contentPageFactory;

        private IList<NavigablePage>? _navigablePages;

        public Page CurrentPage { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ContentPageController(IContentRepository contentRepository, IContentPageFactory contentPageFactory)
        {
            _contentRepository = contentRepository;
            _contentPageFactory = contentPageFactory;
        }

        public async Task LoadNavigation(string name)
        {
            _navigablePages = await _contentRepository.GetNavigationAsync(name);

            if (_navigablePages.Count > 0)
                await LoadContent(_navigablePages[0].Identifier);
        }

        public async Task LoadContent(string identifier)
        {
            try
            {
                var navigablePage = _navigablePages.FirstOrDefault(x => x.Identifier == identifier);
                if (navigablePage == null)
                    throw new EdgeException($"Failed to find page '{identifier}' in navigation");

                var content = await _contentRepository.GetContentAsync(navigablePage.Type, identifier);
                if (content == null)
                {
                    CurrentPage = null;
                }
                else
                {
                    ExtendContent(content);
                    var page = _contentPageFactory.CreatePageForContent(content);
                    CurrentPage = page;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));
            }
            catch (Exception ex)
            {
                throw new EdgeException($"Failed to load content with identifier {identifier}", ex);
            }
        }

        private void ExtendContent(Content content)
        {
            // Find the index of the content in the navigable pages list
            for (var i = 0; i < _navigablePages.Count; i++)
            {
                if (_navigablePages[i].Identifier == content.Identifier)
                {
                    if (i < _navigablePages.Count - 1)
                    {
                        content.NextContentIdentifier = _navigablePages[i + 1].Identifier;
                        content.NextContentType = _navigablePages[i + 1].Type;
                    }

                    return;
                }
            }
        }
    }
}
