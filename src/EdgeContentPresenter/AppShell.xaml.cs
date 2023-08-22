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
        {
            // If the page is already in the nav stack, jump back to it.
            if (Navigation.NavigationStack.Contains(_pageController.CurrentPage))
            {
                while (Navigation.NavigationStack.Last() != _pageController.CurrentPage)
                {
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await Navigation.PushAsync(_pageController.CurrentPage, true);
            }
        }
    }

    private async void OpenSettings(object sender, EventArgs e)
    {
        await Current.GoToAsync(SettingsPageRoute, true);
    }

    private async void OpenPages(object sender, EventArgs e)
    {
        var pages = _pageController.NavigablePages;
        var pageName = await DisplayActionSheet("Navigate to:", "Cancel", null, pages.Select(x => x.Name).ToArray());

        if(!string.IsNullOrEmpty(pageName))
        {
            var page = pages.FirstOrDefault(x => x.Name == pageName);
            if (page != null)
                await _pageController.LoadContent(page.Identifier);
        }
    }

    private async void NextPage(object sender, EventArgs e)
    {
        var currentContent = Current.CurrentPage.BindingContext as Content;
        if (currentContent == null)
            return;

        var nextContent = _pageController.NavigablePages.FindNextPage(currentContent.Identifier);

        if (nextContent == null)
            await DisplayAlert("No Content", "There is no next content", "OK");
        else
        {
            try
            {
                await _pageController.LoadContent(nextContent.Identifier);
            }
            catch (Exception ex)
            {
                await DisplayError(ex);
            }
        }
    }
}
