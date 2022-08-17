using EdgeContentPresenter.Model;
using Microsoft.Extensions.Caching.Memory;

namespace EdgeContentPresenter.ContentTypes
{
    /// <summary>
    /// A decorator for <see cref="IContentPageFactory"/> which caches the pages it creates.
    /// </summary>
    internal class ContentPageFactoryCacheDecorator : IContentPageFactory
    {
        private readonly IContentPageFactory _inner;
        private readonly IMemoryCache _cache;

        public ContentPageFactoryCacheDecorator(IContentPageFactory inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public Page CreatePageForContent(Content content)
        {
            var key = GenerateCacheKey(content.Identifier);

            return _cache.GetOrCreate<Page>(key, entry =>
            {
                var page = _inner.CreatePageForContent(content);
                return page;
            });
        }

        private string GenerateCacheKey(string identifier)
        {
            return "page-" + identifier;
        }
    }
}
