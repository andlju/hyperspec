using System.Collections.Generic;
using System.Linq;

namespace Hyperspec
{
    public class ResourceForm<TTemplate> : ResourceLinkBase<TTemplate>, IForm
    {
        private readonly string _method;

        // TODO Allow overridden template parameter infos
        public ResourceForm(string linkTemplate, IEnumerable<IContentContext> contexts, string title = null, string method = "POST") : 
            base(linkTemplate, contexts, title, null)
        {
            _method = method;
        }

        public override string Href
        {
            get { return GetHref(false); }
        }

        public string Method { get { return _method; } }

        public override IDictionary<string, FormParameterInfo> Template
        {
            get { return GetTemplate(); }
        }

    }

    public class ResourceForm : ResourceLinkBase, IForm
    {
        private readonly string _method;

        // TODO Allow overridden template parameter infos
        public ResourceForm(string linkTemplate, IEnumerable<IContentContext> contexts, string title = null, string method = "POST") :
            base(linkTemplate, contexts, title, null)
        {
            _method = method;
        }

        public override string Href
        {
            get { return GetHref(false); }
        }

        public string Method { get { return _method; } }

        public override IDictionary<string, FormParameterInfo> Template
        {
            get { return GetTemplate(); }
        }
    }

}