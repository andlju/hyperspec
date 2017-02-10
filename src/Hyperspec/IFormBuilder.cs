using System.Collections.Generic;

namespace Hyperspec
{
    public interface IFormBuilder
    {
        void AddForm(string formName, string linkTemplate, string prompt, string method, object context);
        void AddForm(string formName, string linkTemplate, string prompt, string method, IEnumerable<object> contexts);

        void AddForm<TTemplate>(string formName, string linkTemplate, string prompt, string method, object context);
        void AddForm<TTemplate>(string formName, string linkTemplate, string prompt, string method, IEnumerable<object> contexts);
    }
}