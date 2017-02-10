using System.Collections.Generic;

namespace Hyperspec
{
    public class ProfileLink : ILink
    {
        private readonly string _href;

        public ProfileLink(string href)
        {
            _href = href;
        }

        public string Href => _href;

        public string Title => null;

        public IDictionary<string, FormParameterInfo> Template => null;
    }
}