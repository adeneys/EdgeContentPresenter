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
                BlogContent => new BlogContentPage(),
                TitleContent => new TitleContentPage(),
                BioContent => new BioContentPage(),
                SectionTitleContent => new SectionTitlePage(),
                TextContent => new TextContentPage(),
                _ => null
            };

            if(page != null)
                page.BindingContext = content;

            return page;
        }
    }
}
