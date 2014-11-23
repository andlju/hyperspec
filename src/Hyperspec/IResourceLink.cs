using System.Collections.Generic;

namespace Hyperspec
{
    public class ResourceLink<TTemplate> : ResourceLinkBase<TTemplate>
    {
        public ResourceLink(TemplatedLink templatedLink, IEnumerable<object> resources, string title = null)
            : base(templatedLink, resources, title)
        {
        }

        public override string Href
        {
            get { return GetHref(true); }
        }

    }

    public class ResourceLink : ResourceLinkBase
    {
        public ResourceLink(TemplatedLink templatedLink, IEnumerable<object> resources, string title = null)
            : base(templatedLink, resources, title)
        {
        }

        public override string Href
        {
            get { return GetHref(true); }
        }

    }

}