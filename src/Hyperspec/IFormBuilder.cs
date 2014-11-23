namespace Hyperspec
{
    public interface IFormBuilder
    {
        void AddForm<TTemplate>(string formName, string linkTemplate, string prompt = null, string method = "POST", object context = null);
    }
}