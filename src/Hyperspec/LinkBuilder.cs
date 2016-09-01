using System;
using System.Collections.Generic;
using System.Linq;

namespace Hyperspec
{
    class LinkBuilder : ILinkBuilder
    {
        private readonly IEnumerable<IContentContext> _contexts;
        private readonly string _linkBase;

        public IDictionary<string, IList<ILink>> Links { get; private set; }

        public LinkBuilder(IEnumerable<IContentContext> contexts, string linkBase)
        {
            _contexts = contexts;
            _linkBase = linkBase.EndsWith("/") ? linkBase : linkBase + "/"; // Always end the base with a /
            Links = new Dictionary<string, IList<ILink>>();
        }

        public void AddLink<TTemplate>(string linkName, string linkTemplate, string prompt = null, object content = null, IEnumerable<ResourceLinkBase.TemplateParameterInfo> parameterInfos = null)
        {
            var contexts = _contexts;
            if (content != null)
                contexts = new[] { new ContentContext(content) }.Concat(_contexts);

            linkTemplate = RebaseLink(linkTemplate);

            ILink link = new ResourceLink<TTemplate>(linkTemplate, contexts, prompt, parameterInfos);

            AddNamedLink(linkName, link);
        }

        private string RebaseLink(string linkTemplate)
        {
            if (!linkTemplate.Contains("//"))
            {
                linkTemplate = linkTemplate.TrimStart('/'); // Since the base always ends with a /, make sure to remove any from the template

                linkTemplate = _linkBase + linkTemplate;
            }
            return linkTemplate;
        }

        public void AddLink(string linkName, string linkTemplate, string prompt = null, object content = null, IEnumerable<ResourceLinkBase.TemplateParameterInfo> parameterInfos = null)
        {
            var contexts = _contexts;
            if (content != null)
                contexts = new[] { new ContentContext(content) }.Concat(_contexts);

            linkTemplate = RebaseLink(linkTemplate);
            
            ILink link = new ResourceLink(linkTemplate, contexts, prompt, parameterInfos);

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