namespace Hyperspec
{
    public interface IResourceLinkBuilder
    {
        void AddLink(string linkName, Link link, string prompt = null, object context = null);
        void AddLink<TTemplate>(string linkName, Link link, string prompt = null, object context = null);
    }
}