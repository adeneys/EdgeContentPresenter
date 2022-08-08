using EdgeContentPresenter.Model;

namespace EdgeContentPresenter.ContentSource
{
    public interface IContentRepository
    {
        Task<Content?> GetContentAsync(string identifier);
    }
}
