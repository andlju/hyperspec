using System.Collections.Generic;

namespace Hyperspec
{
    public class ResourceLink : ResourceLinkBase
    {
        private readonly bool _includeTemplateDetails;

        public ResourceLink(string linkTemplate, IEnumerable<IContentContext> contexts, string title, bool includeTemplateDetails = false, IEnumerable<TemplateParameterInfo> parameterInfos = null)
            : base(linkTemplate, contexts, title, parameterInfos)
        {
            _includeTemplateDetails = includeTemplateDetails;
        }

        public override string Href
        {
            get { return GetHref(true); }
        }

        public override IDictionary<string, FormParameterInfo> Template
        {
            get { return _includeTemplateDetails ? GetTemplate() : null; }
        }

    }

    public class ResourceLink<TTemplate> : ResourceLinkBase<TTemplate>
    {
        private readonly bool _includeTemplateDetails;

        public ResourceLink(string linkTemplate, IEnumerable<IContentContext> contexts, string title, bool includeTemplateDetails = false, IEnumerable<TemplateParameterInfo> parameterInfos = null)
            : base(linkTemplate, contexts, title, parameterInfos)
        {
            _includeTemplateDetails = includeTemplateDetails;
        }

        public override string Href
        {
            get { return GetHref(true); }
        }

        public override IDictionary<string, FormParameterInfo> Template
        {
            get { return _includeTemplateDetails ? GetTemplate() : null; }
        }

    }
}