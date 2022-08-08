using EdgeContentPresenter.ContentTypes;
using System.ComponentModel;

namespace EdgeContentPresenter.ContentSource
{
    internal class ContentPageController : IContentPageController
    {
        private readonly IContentRepository _contentRepository;
        private readonly IContentPageFactory _contentPageFactory;

        public Page CurrentPage { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ContentPageController(IContentRepository contentRepository, IContentPageFactory contentPageFactory)
        {
            _contentRepository = contentRepository;
            _contentPageFactory = contentPageFactory;
        }

        public async Task LoadContent(string identifier)
        {
            try
            {
                var content = await _contentRepository.GetContentAsync(identifier);
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
