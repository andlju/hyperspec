using System.Collections.Generic;

namespace Hyperspec
{
    public interface IForm : ILink
    {
        string Method { get; }
        IDictionary<string, FormParameterInfo> Template { get; }
    }
}