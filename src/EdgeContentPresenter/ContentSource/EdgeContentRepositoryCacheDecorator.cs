using EdgeContentPresenter.Model;
using Microsoft.Extensions.Caching.Memory;

namespace EdgeContentPresenter.ContentSource
{
    /// <summary>
    /// A decorator for <see cref="IContentRepository"/> which caches content.
    /// </summary>
    internal class EdgeContentRepositoryCacheDecorator : IContentRepository
    {
        private readonly IContentRepository _inner;
        private readonly IMemoryCache _cache;

        public EdgeContentRepositoryCacheDecorator(IContentRepository inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task<Content> GetContentAsync(string identifier)
        {
            var key = GenerateCacheKey(identifier);
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                var content = await _inner.GetContentAsync(identifier);
                return content;
            });
        }

        private string GenerateCacheKey(string identifier)
        {
            return "content-" + identifier;
        }
    }
}
