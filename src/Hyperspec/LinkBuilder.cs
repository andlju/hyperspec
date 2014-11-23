using System.Collections.Generic;
using System.Linq;

namespace Hyperspec
{
    class LinkBuilder : ILinkBuilder
    {
        private readonly IEnumerable<object> _contexts;
        public IDictionary<string, IList<ILink>> Links { get; private set; }

        public LinkBuilder(IEnumerable<object> contexts)
        {
            _contexts = contexts;
            Links = new Dictionary<string, IList<ILink>>();
        }

        public void AddLink<TTemplate>(string linkName, TemplatedLink templatedLink, string prompt = null, object context = null)
        {
            var contexts = _contexts;
            if (context != null)
                contexts = new[] { context }.Concat(_contexts);

            ILink link = new ResourceLink<TTemplate>(templatedLink, contexts, prompt);

            AddNamedLink(linkName, link);
        }

        public void AddLink(string linkName, TemplatedLink templatedLink, string prompt = null, object context = null)
        {
            var contexts = _contexts;
            if (context != null)
                contexts = new[] { context }.Concat(_contexts);
            ILink link = new ResourceLink(templatedLink, contexts, prompt);

            AddNamedLink(linkName, link);
        }

        public void AddLink(string linkName, string href, string prompt = null)
        {
            AddNamedLink(linkName, new StaticLink(href, prompt));
        }

        public void AddSelfLink<TTemplate>(TemplatedLink selfTemplatedLink)
        {
            AddLink<TTemplate>("self", selfTemplatedLink);
        }

        public void AddSelfLink(TemplatedLink selfTemplatedLink)
        {
            AddLink("self", selfTemplatedLink);
        }

        public void AddProfileLink(string href)
        {
            AddNamedLink("profile", new ProfileLink(href));
        }

        private void AddNamedLink(string linkName, ILink link)
        {
            IList<ILink> linkList;
            if (!Links.TryGetValue(linkName, out linkList))
            {
                linkList = new List<ILink>();
                Links.Add(linkName, linkList);
            }
            linkList.Add(link);
        }
    }
}