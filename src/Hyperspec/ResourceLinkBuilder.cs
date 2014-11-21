using System.Collections.Generic;
using System.Linq;

namespace Hyperspec
{
    class ResourceLinkBuilder : IResourceLinkBuilder
    {
        private readonly IEnumerable<object> _contexts;
        public IDictionary<string, IList<IResourceLink>> Links { get; private set; }

        public ResourceLinkBuilder(IEnumerable<object> contexts)
        {
            _contexts = contexts;
            Links = new Dictionary<string, IList<IResourceLink>>();
        }

        public void AddLink<TTemplate>(string linkName, Link link, string prompt = null, object context = null)
        {
            var contexts = _contexts;
            if (context != null)
                contexts = new[] { context }.Concat(_contexts);

            IResourceLink resourceLink = new ResourceLink<TTemplate>(link, contexts, prompt);

            AddResourceLink(linkName, resourceLink);
        }

        public void AddLink(string linkName, Link link, string prompt = null, object context = null)
        {
            var contexts = _contexts;
            if (context != null)
                contexts = new[] { context }.Concat(_contexts);
            IResourceLink resourceLink = new ResourceLink(link, contexts, prompt);

            AddResourceLink(linkName, resourceLink);
        }

        public void AddSelfLink<TTemplate>(Link selfLink)
        {
            AddLink<TTemplate>("self", selfLink);
        }

        public void AddSelfLink(Link selfLink)
        {
            AddLink("self", selfLink);
        }

        public void AddProfileLink(string href)
        {
            AddResourceLink("profile", new ProfileLink(href));
        }

        private void AddResourceLink(string linkName, IResourceLink resourceLink)
        {
            IList<IResourceLink> linkList;
            if (!Links.TryGetValue(linkName, out linkList))
            {
                linkList = new List<IResourceLink>();
                Links.Add(linkName, linkList);
            }
            linkList.Add(resourceLink);
        }
    }
}