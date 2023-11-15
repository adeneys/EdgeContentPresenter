using EdgeContentPresenter.Model;

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

        // Adjust height request of headerImage once it starts stretching
        var pageHeaderImage = ((BioContent)BindingContext).PageHeaderImage;

        if (pageHeaderImage != null)
        {
            headerImage.TranslationX = width - pageHeaderImage.Width;
        }
    }
}