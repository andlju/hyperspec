using System.Collections.Generic;

namespace Hyperspec
{
    public class ResourceLink<TTemplate> : ResourceLinkBase<TTemplate>, ILink
    {
        public ResourceLink(TemplatedLink templatedLink, IEnumerable<object> resources, string title = null)
            : base(templatedLink, resources, title)
        {
        }

        public string Href
        {
            get { return GetHref(true); }
        }

    }

    public class ResourceLink : ResourceLinkBase, ILink
    {
        public ResourceLink(TemplatedLink templatedLink, IEnumerable<object> resources, string title = null)
            : base(templatedLink, resources, title)
        {
        }

        public string Href
        {
            get { return GetHref(true); }
        }

    }

}