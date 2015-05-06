using System;
using System.Collections.Generic;
using System.Linq;

namespace Hyperspec
{
    class LinkBuilder : ILinkBuilder
    {
        private readonly IEnumerable<IContentContext> _contexts;
        public IDictionary<string, IList<ILink>> Links { get; private set; }
        private readonly string _linkBase;

        public LinkBuilder(IEnumerable<IContentContext> contexts, string linkBase)
        {
            _contexts = contexts;
            _linkBase = linkBase;
            Links = new Dictionary<string, IList<ILink>>();
        }

        public void AddLink<TTemplate>(string linkName, string linkTemplate, string prompt = null, object content = null)
        {
            var contexts = _contexts;
            if (content != null)
                contexts = new[] { new ContentContext(content) }.Concat(_contexts);

            if (!linkTemplate.Contains("://"))
            {
                linkTemplate = _linkBase + linkTemplate;
            }
            ILink link = new ResourceLink<TTemplate>(linkTemplate, contexts, prompt);

            AddNamedLink(linkName, link);
        }

        public void AddLink(string linkName, string linkTemplate, string prompt = null, object content = null)
        {
            var contexts = _contexts;
            if (content != null)
                contexts = new[] { new ContentContext(content) }.Concat(_contexts);
            if (!linkTemplate.Contains("://"))
            {
                linkTemplate = _linkBase + linkTemplate;
            }
            ILink link = new ResourceLink(linkTemplate, contexts, prompt);

            AddNamedLink(linkName, link);
        }

        public void AddSelfLink<TTemplate>(string linkTemplate)
        {
            AddLink<TTemplate>("self", linkTemplate);
        }

        public void AddSelfLink(string linkTemplate)
        {
            AddLink("self", linkTemplate);
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