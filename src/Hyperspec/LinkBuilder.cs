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

        public void AddLink<TTemplate>(string linkName, string linkTemplate, string prompt = null)
        {
            AddLink<TTemplate>(linkName, linkTemplate, prompt, new object[0]);
        }

        public void AddLink<TTemplate>(string linkName, string linkTemplate, string prompt, object context,
            IEnumerable<ResourceLinkBase.TemplateParameterInfo> parameterInfos = null)
        {
            AddLink<TTemplate>(linkName, linkTemplate, prompt, new[] {context}, parameterInfos);
        }

        public void AddLink<TTemplate>(string linkName, string linkTemplate, string prompt, IEnumerable<object> contexts, IEnumerable<ResourceLinkBase.TemplateParameterInfo> parameterInfos = null)
        {
            var linkContexts = _contexts;
            if (contexts != null)
            {
                linkContexts = contexts.Select(c => new ContentContext(c)).Concat(_contexts);
            }

            linkTemplate = RebaseLink(linkTemplate);

            ILink link = new ResourceLink<TTemplate>(linkTemplate, linkContexts, prompt, parameterInfos);

            AddNamedLink(linkName, link);
        }

        public void AddLink(string linkName, string linkTemplate, string prompt = null)
        {
            AddLink(linkName, linkTemplate, prompt, new object[0]);
        }

        public void AddLink(string linkName, string linkTemplate, string prompt, object context,
            IEnumerable<ResourceLinkBase.TemplateParameterInfo> parameterInfos = null)
        {
            AddLink(linkName, linkTemplate, prompt, new[] { context }, parameterInfos);
        }

        public void AddLink(string linkName, string linkTemplate, string prompt, IEnumerable<object> contexts, IEnumerable<ResourceLinkBase.TemplateParameterInfo> parameterInfos = null)
        {
            var linkContexts = _contexts;
            if (contexts != null)
            {
                linkContexts = contexts.Select(c => new ContentContext(c)).Concat(_contexts);
            }

            linkTemplate = RebaseLink(linkTemplate);
            
            ILink link = new ResourceLink(linkTemplate, linkContexts, prompt, parameterInfos);

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