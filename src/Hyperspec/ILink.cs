using System.Collections.Generic;

namespace Hyperspec
{
    public interface ILink
    {
        string Href { get; }
        string Title { get; }

        IDictionary<string, FormParameterInfo> Template { get; }
    }
}