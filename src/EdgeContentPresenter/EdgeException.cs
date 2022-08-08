namespace EdgeContentPresenter
{
    internal class EdgeException : Exception
    {
        public EdgeException(string message) : base(message)
        {
        }

        public EdgeException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
