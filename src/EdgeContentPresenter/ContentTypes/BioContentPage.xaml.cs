namespace EdgeContentPresenter.ContentTypes;

public partial class BioContentPage : ContentPage
{
	public BioContentPage()
	{
		InitializeComponent();

#if ANDROID
		// The header image is too bit for Android to handle and crashes the app. Just remove it for now.
		headerImage.Source = "";
#endif
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        headerImage.WidthRequest = width;
    }
}