namespace EdgeContentPresenter;

public partial class App : Application
{
    public App(AppShell appShell)
    {
        InitializeComponent();
        MainPage = appShell;

        AppDomain.CurrentDomain.UnhandledException += ExceptionHandler;
    }

    private void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
    {
        var y = 0;
    }
}
