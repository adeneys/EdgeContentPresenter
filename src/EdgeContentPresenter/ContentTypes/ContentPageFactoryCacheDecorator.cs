using EdgeContentPresenter.Model;

namespace EdgeContentPresenter.ContentTypes
{
    /// <summary>
    /// A decorator for <see cref="IContentPageFactory"/> which caches the pages it creates.
    /// </summary>
    internal class ContentPageFactoryCacheDecorator : IContentPageFactory
    {
        private readonly IContentPageFactory _inner;
        private readonly Dictionary<string, Page> _cache;

        public ContentPageFactoryCacheDecorator(IContentPageFactory inner)
        {
            _inner = inner;
            _cache = new Dictionary<string, Page>();
        }

        public Page CreatePageForContent(Content content)
        {
            if (_cache.ContainsKey(content.Identifier))
                return _cache[content.Identifier];

            var page = _inner.CreatePageForContent(content);
            _cache.Add(content.Identifier, page);

            return page;
        }
    }
}
