using System.Collections.Generic;
using System.Linq;

namespace Hyperspec
{
    public class ResourceForm<TTemplate> : ResourceLinkBase<TTemplate>, IForm
    {
        private readonly string _method;

        public ResourceForm(string linkTemplate, IEnumerable<IContentContext> contexts, string title = null, string method = "POST") : 
            base(linkTemplate, contexts, title)
        {
            _method = method;
        }

        public override string Href
        {
            get { return GetHref(false); }
        }

        public string Method { get { return _method; } }

        public IDictionary<string, FormParameterInfo> Template
        {
            get { return GetTemplate(); }
        }

        private IDictionary<string, FormParameterInfo> GetTemplate()
        {
            return ParameterInfos.Values.Where(pi => !pi.InTemplate).ToDictionary(pi => pi.Name, pi => new FormParameterInfo()
            {
                Title = pi.Title,
                Type = pi.Type,
                IsRequired = pi.IsRequired,
                DefaultValue = GetParameter(Contexts, pi.Name).FirstOrDefault()
            });
        }
    }
}