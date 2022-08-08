using EdgeContentPresenter.ContentSource;
using EdgeContentPresenter.Settings;

namespace EdgeContentPresenter;

public partial class App : Application
{
    private readonly AppShell _appShell;
    private readonly IContentPageController _pageController;
    private readonly IAppSettings _appSettings;


    public App(AppShell appShell, IContentPageController pageController, IAppSettings appSettings)
	{
        _appShell = appShell;
        _pageController = pageController;
        _appSettings = appSettings;

        InitializeComponent();

		MainPage = appShell;
    }

	protected override async void OnStart()
	{
		base.OnStart();

        try
        {
            var startIdentifier = await _appSettings.GetTitleContentIdentifierAsync();
            await _pageController.LoadContent(startIdentifier);
        }
        catch(Exception ex)
        {
            await _appShell.DisplayError(ex);
        }
	}
}
