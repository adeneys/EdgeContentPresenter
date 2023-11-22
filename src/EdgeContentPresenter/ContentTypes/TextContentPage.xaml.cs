using EdgeContentPresenter.Model;

namespace EdgeContentPresenter.ContentTypes;

public partial class TextContentPage : ContentPage
{
	public TextContentPage()
	{
		InitializeComponent();

#if ANDROID
        // The header image is too big for Android to handle and crashes the app. Just remove it for now.
        headerImage.Source = "";
#endif
    }

    public int MaximumImageWidth => ((TextContent)BindingContext).MaximumImageWidth;

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        headerImage.WidthRequest = width;
    }
}
