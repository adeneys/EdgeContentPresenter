namespace EdgeContentPresenter.Model
{
    internal static class NavigablePageExtensions
    {
        public static NavigablePage? FindNextPage(this IList<NavigablePage> pages, string currentPageIdentifier)
        {
            for(var i = 0; i < pages.Count; i++)
            {
                if (pages[i].Identifier == currentPageIdentifier && i < pages.Count - 1)
                    return pages[i + 1];
            }

            return null;
        }
    }
}
