using System.Collections.Generic;
using System.Linq;

namespace Hyperspec
{
    class FormBuilder : IFormBuilder
    {
        public IDictionary<string, IList<IForm>> Forms { get; private set; }

        private readonly IEnumerable<IContentContext> _contexts;
        private readonly string _linkBase;

        public FormBuilder(IEnumerable<IContentContext> contexts, string linkBase)
        {
            _contexts = contexts;
            _linkBase = linkBase.EndsWith("/") ? linkBase : linkBase + "/"; // Always end the base with a /
            Forms = new Dictionary<string, IList<IForm>>();
        }

        public void AddForm<TTemplate>(string formName, string linkTemplate, string prompt = null, string method = "POST", object context = null)
        {
            IList<IForm> formList;
            if (!Forms.TryGetValue(formName, out formList))
            {
                formList = new List<IForm>();
                Forms.Add(formName, formList);
            }
            var contexts = _contexts;
            if (context != null)
                contexts = new[] { new ContentContext(context) }.Concat(_contexts);

            linkTemplate = RebaseLink(linkTemplate);

            formList.Add(new ResourceForm<TTemplate>(linkTemplate, contexts, prompt, method));
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

    }
}