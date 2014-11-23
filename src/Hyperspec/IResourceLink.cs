using System.Collections.Generic;

namespace Hyperspec
{
    public class ResourceLink<TTemplate> : ResourceLinkBase<TTemplate>
    {
        public ResourceLink(string linkTemplate, IEnumerable<object> resources, string title = null)
            : base(linkTemplate, resources, title)
        {
        }

        public override string Href
        {
            get { return GetHref(true); }
        }

    }

    public class ResourceLink : ResourceLinkBase
    {
        public ResourceLink(string linkTemplate, IEnumerable<object> resources, string title = null)
            : base(linkTemplate, resources, title)
        {
        }

        public override string Href
        {
            get { return GetHref(true); }
        }

    }

}