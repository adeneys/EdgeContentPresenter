using EdgeContentPresenter.ContentTypes;
using EdgeContentPresenter.Model;
using System.ComponentModel;

namespace EdgeContentPresenter.ContentSource
{
    internal class ContentPageController : IContentPageController
    {
        private readonly IContentRepository _contentRepository;
        private readonly IContentPageFactory _contentPageFactory;

        public IList<NavigablePage>? NavigablePages { get; private set; }
        public Page CurrentPage { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ContentPageController(IContentRepository contentRepository, IContentPageFactory contentPageFactory)
        {
            _contentRepository = contentRepository;
            _contentPageFactory = contentPageFactory;
        }

        public async Task LoadNavigation(string name)
        {
            NavigablePages = await _contentRepository.GetNavigationAsync(name);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavigablePages)));

            if (NavigablePages.Count > 0)
                await LoadContent(NavigablePages[0].Identifier);
        }

        public async Task LoadContent(string identifier)
        {
            try
            {
                var navigablePage = NavigablePages.FirstOrDefault(x => x.Identifier == identifier);
                if (navigablePage == null)
                    throw new EdgeException($"Failed to find page '{identifier}' in navigation");

                var content = await _contentRepository.GetContentAsync(navigablePage.Type, identifier);
                if (content == null)
                {
                    CurrentPage = null;
                }
                else
                {
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
    }
}
