namespace Hyperspec
{
    public interface IFormBuilder
    {
        void AddForm<TTemplate>(string formName, TemplatedLink templatedLink, string prompt = null, string method = "POST", object context = null);
    }
}