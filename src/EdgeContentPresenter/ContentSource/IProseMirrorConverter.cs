namespace EdgeContentPresenter.ContentSource
{
    /// <summary>
    /// Defines a type which converts ProseMirror (https://prosemirror.net/) content.
    /// </summary>
    internal interface IProseMirrorConverter
    {
        /// <summary>
        /// Converts a ProseMirror document to compatible .net MAUI Label HTML.
        /// </summary>
        /// <param name="proseMirrorJsonText">JSON text for a ProseMirror document.</param>
        /// <returns>HTML for use in a .net MAUI label.</returns>
        string ToMauiHtml(string proseMirrorJsonText);
    }
}
