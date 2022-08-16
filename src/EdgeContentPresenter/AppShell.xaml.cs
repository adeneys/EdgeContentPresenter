using EdgeContentPresenter.ContentSource;
using EdgeContentPresenter.Model;
using System.ComponentModel;

namespace EdgeContentPresenter;

public partial class AppShell : Shell
{
    private readonly string SettingsPageRoute = "Settings";

    private readonly IContentPageController _pageController;

    public AppShell(IContentPageController pageController)
    {
        _pageController = pageController;

        InitializeComponent();

        Routing.RegisterRoute(SettingsPageRoute, typeof(SettingsPage));

        _pageController.PropertyChanged += PageControllerPropertyChange;
    }

    public async Task DisplayError(Exception ex)
    {
        await DisplayAlert("Error", ex.ToString(), "OK");
    }

    private async void PageControllerPropertyChange(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IContentPageController.CurrentPage))
            await Navigation.PushAsync(_pageController.CurrentPage, true);
    }

    private async void OpenSettings(object sender, EventArgs e)
    {
        await Current.GoToAsync(SettingsPageRoute, true);
    }

    private async void NextPage(object sender, EventArgs e)
    {
        var currentContent = Current.CurrentPage.BindingContext as Content;
        if (currentContent == null)
            return;

        if (currentContent.NextContentIdentifier == null)
            await DisplayAlert("No Content", "There is no next content", "OK");
        else
        {
            try
            {
                await _pageController.LoadContent(currentContent.NextContentIdentifier);
            }
            catch (Exception ex)
            {
                await DisplayError(ex);
            }
        }
    }
}
