namespace Hyperspec
{
    public interface IResourceFormBuilder
    {
        void AddForm<TTemplate>(string formName, Link link, string prompt = null, string method = "POST", object context = null);
    }
}