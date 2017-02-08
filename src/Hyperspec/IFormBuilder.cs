namespace Hyperspec
{
    public interface IFormBuilder
    {
        void AddForm(string formName, string linkTemplate, string prompt = null, string method = "POST", object context = null);
        void AddForm<TTemplate>(string formName, string linkTemplate, string prompt = null, string method = "POST", object context = null);
    }
}