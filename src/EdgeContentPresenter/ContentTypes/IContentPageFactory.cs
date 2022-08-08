using EdgeContentPresenter.Model;

namespace EdgeContentPresenter.ContentTypes
{
    public interface IContentPageFactory
    {
        Page? CreatePageForContent(Content content);
    }
}
