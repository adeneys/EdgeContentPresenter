using EdgeContentPresenter.Model;

namespace EdgeContentPresenter.ContentSource
{
    internal interface IContentMapper
    {
        IList<NavigablePage> MapNavigationResponse(string content);

        Content? MapContentResponse(string content);
    }
}
