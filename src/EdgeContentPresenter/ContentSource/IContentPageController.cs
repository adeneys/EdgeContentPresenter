using System.ComponentModel;

namespace EdgeContentPresenter.ContentSource
{
    public interface IContentPageController : INotifyPropertyChanged
    {
        Page? CurrentPage { get; }

        Task LoadContent(string identifier);
    }
}
