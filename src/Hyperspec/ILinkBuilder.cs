namespace Hyperspec
{
    public interface ILinkBuilder
    {
        void AddLink(string linkName, TemplatedLink templatedLink, string prompt = null, object context = null);
        void AddLink<TTemplate>(string linkName, TemplatedLink templatedLink, string prompt = null, object context = null);
        void AddLink(string linkName, string href, string prompt = null);
        void AddSelfLink<TTemplate>(TemplatedLink selfTemplatedLink);
        void AddSelfLink(TemplatedLink selfTemplatedLink);
        void AddProfileLink(string href);
    }
}