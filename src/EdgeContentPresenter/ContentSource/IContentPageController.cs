using EdgeContentPresenter.Model;
using System.ComponentModel;

namespace EdgeContentPresenter.ContentSource
{
    public interface IContentPageController : INotifyPropertyChanged
    {
        IList<NavigablePage>? NavigablePages { get; }

        Page? CurrentPage { get; }

        Task LoadNavigation(string name);

        Task LoadContent(string identifier);
    }
}
