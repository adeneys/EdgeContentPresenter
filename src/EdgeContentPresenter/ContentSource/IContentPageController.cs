using System.ComponentModel;

namespace EdgeContentPresenter.ContentSource
{
    public interface IContentPageController : INotifyPropertyChanged
    {
        Page? CurrentPage { get; }

        Task LoadNavigation(string name);

        Task LoadContent(string identifier);
    }
}
