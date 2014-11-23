using System.Collections.Generic;
using System.Linq;

namespace Hyperspec
{
    public class ResourceForm<TTemplate> : ResourceLinkBase<TTemplate>, IResourceForm
    {
        private readonly string _method;

        public ResourceForm(TemplatedLink templatedLink, IEnumerable<object> resources, string title = null, string method = "POST") : base(templatedLink, resources, title)
        {
            _method = method;
        }

        public string Href
        {
            get { return GetHref(false); }
        }

        public string Method { get { return _method; } }

        public IDictionary<string, ParameterInfo> Template
        {
            get { return GetTemplate(); }
        }

        private IDictionary<string, ParameterInfo> GetTemplate()
        {
            return ParameterInfos.Values.Where(pi => !pi.InTemplate).ToDictionary(pi => pi.Name, pi => new ParameterInfo()
            {
                Title = pi.Title,
                Type = pi.Type,
                IsRequired = pi.IsRequired,
                DefaultValue = GetParameter(Resources, pi.Name)
            });
        }
    }
}