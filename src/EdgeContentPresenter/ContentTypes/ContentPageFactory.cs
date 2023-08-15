using EdgeContentPresenter.Model;

namespace EdgeContentPresenter.ContentTypes
{
    internal class ContentPageFactory : IContentPageFactory
    {
        public Page? CreatePageForContent(Content content)
        {
            if (content == null)
                return null;

            ContentPage page = content switch
            {
                TitleContent => new TitleContentPage(),
                BioContent => new BioContentPage(),
                SectionTitleContent sectionTitleContent when sectionTitleContent.Layout == "taxonomy_layout_left" => new SectionSubtitlePageLeft(),
                SectionTitleContent sectionTitleContent when sectionTitleContent.Layout == "taxonomy_layout_right" => new SectionSubtitlePageRight(),
                SectionTitleContent => new SectionTitlePageLeft(),
                TextContent => new TextContentPage(),
                SocialContent => new SocialContentPage(),
                _ => null
            };

            if(page != null)
                page.BindingContext = content;

            return page;
        }
    }
}
