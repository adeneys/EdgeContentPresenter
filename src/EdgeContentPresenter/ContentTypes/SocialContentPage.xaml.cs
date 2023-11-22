namespace EdgeContentPresenter.ContentTypes;

public partial class SocialContentPage : ContentPage
{
	public SocialContentPage()
	{
		InitializeComponent();
	}

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        Labels.TranslationX = (width - Labels.Width) * 0.5;
        Labels.TranslationY = (height - Labels.Height) * 0.4;
    }
}