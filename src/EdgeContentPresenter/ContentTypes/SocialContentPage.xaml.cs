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
        Labels.TranslationX = width * 0.22;
        Labels.TranslationY = height * 0.61;
        Labels.Spacing = width * 0.08;
    }
}