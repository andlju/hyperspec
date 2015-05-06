using System.Collections.Generic;

namespace Hyperspec
{
    public class ResourceLink : ResourceLinkBase
    {
        public ResourceLink(string linkTemplate, IEnumerable<IContentContext> contexts, string title = null)
            : base(linkTemplate, contexts, title)
        {
        }

        public override string Href
        {
            get { return GetHref(true); }
        }

    }

    public class ResourceLink<TTemplate> : ResourceLinkBase<TTemplate>
    {
        public ResourceLink(string linkTemplate, IEnumerable<IContentContext> contexts, string title = null)
            : base(linkTemplate, contexts, title)
        {
        }

        public override string Href
        {
            get { return GetHref(true); }
        }

    }
}