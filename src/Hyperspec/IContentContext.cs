using System;

namespace Hyperspec
{
    public interface IContentContext
    {
        object Content { get; }
        bool IncludeProperty(string name, object value);
    }

    public class ContentContext : IContentContext
    {
        private readonly object _content;
        private readonly Func<string, object, bool> _includeFunc;

        public ContentContext(object content, Func<string, object, bool> includeFunc = null)
        {
            _content = content;
            _includeFunc = includeFunc;
        }

        public object Content
        {
            get { return _content; }
        }

        public bool IncludeProperty(string name, object value)
        {
            if (_includeFunc == null)
                return true;

            return _includeFunc(name, value);
        }
    }
}