using EdgeContentPresenter.Model;

namespace EdgeContentPresenter.ContentSource
{
    public interface IContentRepository
    {
        Task<IList<NavigablePage>> GetNavigationAsync(string name);

        Task<Content?> GetContentAsync(string identifier);
    }
}
