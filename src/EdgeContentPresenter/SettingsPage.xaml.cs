using EdgeContentPresenter.Settings;

namespace EdgeContentPresenter
{
    public partial class SettingsPage : ContentPage
    {
        private readonly IAppSettings _appSettings;

        public SettingsPage(IAppSettings appSettings)
        {
            _appSettings = appSettings;
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            url.Text = await _appSettings.GetEdgeUrlAsync();
            token.Text = await _appSettings.GetXGqlTokenAsync();
            startIdentifier.Text = await _appSettings.GetTitleContentIdentifierAsync();
        }

        protected async void SaveSettings(object sender, EventArgs e)
        {
            await _appSettings.SetEdgeUrlAsync(url.Text);
            await _appSettings.SetXGqlTokenAsync(token.Text);
            await _appSettings.SetTitleContentIdentifierAsync(startIdentifier.Text);
        }
    }
}