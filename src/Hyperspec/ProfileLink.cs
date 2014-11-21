namespace Hyperspec
{
    public class ProfileLink : IResourceLink
    {
        private readonly string _href;

        public ProfileLink(string href)
        {
            _href = href;
        }

        public string Href { get { return _href; } }
        public string Title { get { return null; } }
    }
}