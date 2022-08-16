using EdgeContentPresenter.ContentSource;
using EdgeContentPresenter.ContentTypes;
using EdgeContentPresenter.Settings;

namespace EdgeContentPresenter;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<IAppSettings, AppSettings>();
        builder.Services.AddSingleton<IContentMapper, ContentMapper>();
        builder.Services.AddSingleton<IContentRepository, EdgeContentRepository>();
        builder.Services.AddSingleton<IContentPageFactory, ContentPageFactory>();
        builder.Services.AddSingleton<IContentPageController, ContentPageController>();

        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<SettingsPage>();

        builder.Services.Decorate<IContentPageFactory, ContentPageFactoryCacheDecorator>();
        builder.Services.Decorate<IContentPageController, ContentPageControllerPreFetchDecorator>();

        return builder.Build();
    }
}
