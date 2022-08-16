﻿using EdgeContentPresenter.ContentSource;
using EdgeContentPresenter.Settings;

namespace EdgeContentPresenter;

public partial class MainPage : ContentPage
{
    private readonly IContentPageController _pageController;
    private readonly IAppSettings _appSettings;
    private readonly AppShell _appShell;

    public MainPage(IContentPageController pageController, IAppSettings appSettings, AppShell appShell)
    {
        InitializeComponent();

        _pageController = pageController;
        _appSettings = appSettings;
        _appShell = appShell;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        try
        {
            var startIdentifier = await _appSettings.GetTitleContentIdentifierAsync();
            await _pageController.LoadContent(startIdentifier);
        }
        catch (Exception ex)
        {
            await _appShell.DisplayError(ex);
        }
    }
}

