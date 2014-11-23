namespace Hyperspec
{
    public class StaticLink : ILink
    {
        private readonly string _href;
        private readonly string _title;

        public StaticLink(string href, string title)
        {
            _href = href;
            _title = title;
        }

        public string Href
        {
            get { return _href; }
        }

        public string Title
        {
            get { return _title; }
        }
    }
}