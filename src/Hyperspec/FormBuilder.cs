using System.Collections.Generic;
using System.Linq;

namespace Hyperspec
{
    class FormBuilder : IFormBuilder
    {
        public IDictionary<string, IList<IResourceForm>> Forms { get; private set; }
        private readonly IEnumerable<object> _contexts;
        public FormBuilder(IEnumerable<object> contexts)
        {
            _contexts = contexts;
            Forms = new Dictionary<string, IList<IResourceForm>>();
        }

        public void AddForm<TTemplate>(string formName, TemplatedLink templatedLink, string prompt = null, string method = "POST", object context = null)
        {
            IList<IResourceForm> formList;
            if (!Forms.TryGetValue(formName, out formList))
            {
                formList = new List<IResourceForm>();
                Forms.Add(formName, formList);
            }
            var contexts = _contexts;
            if (context != null)
                contexts = new[] { context }.Concat(_contexts);

            formList.Add(new ResourceForm<TTemplate>(templatedLink, contexts, prompt, method));
        }
    }
}