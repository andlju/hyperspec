using System.Collections.Generic;

namespace Hyperspec
{
    public interface IResourceLink
    {
        string Href { get; }
        string Title { get; }
    }

    public class ResourceLink<TTemplate> : ResourceLinkBase<TTemplate>, IResourceLink
    {
        public ResourceLink(Link link, IEnumerable<object> resources, string title = null)
            : base(link, resources, title)
        {
        }

        public string Href
        {
            get { return GetHref(true); }
        }

    }

    public class ResourceLink : ResourceLinkBase, IResourceLink
    {
        public ResourceLink(Link link, IEnumerable<object> resources, string title = null)
            : base(link, resources, title)
        {
        }

        public string Href
        {
            get { return GetHref(true); }
        }

    }

}