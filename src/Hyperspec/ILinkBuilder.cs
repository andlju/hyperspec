namespace Hyperspec
{
    public interface ILinkBuilder
    {
        void AddLink(string linkName, string linkTemplate, string prompt = null, object context = null);
        void AddLink<TTemplate>(string linkName, string linkTemplate, string prompt = null, object context = null);
        void AddSelfLink<TTemplate>(string selfTemplatedLink);
        void AddSelfLink(string selfTemplatedLink);
        void AddProfileLink(string href);
    }
}