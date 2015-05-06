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
            _linkBase = linkBase;
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

            if (!linkTemplate.Contains("://"))
            {
                linkTemplate = _linkBase + linkTemplate;
            }

            formList.Add(new ResourceForm<TTemplate>(linkTemplate, contexts, prompt, method));
        }
    }
}