using System.Collections.Generic;

namespace Hyperspec
{
    public interface IResourceForm
    {
        string Href { get; }
        string Method { get; }
        IDictionary<string, ParameterInfo> Template { get; }
    }
    public class ParameterInfo
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; }
    }

}