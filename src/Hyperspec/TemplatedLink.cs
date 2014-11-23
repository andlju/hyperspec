using System.Text.RegularExpressions;
using Resta.UriTemplates;

namespace Hyperspec
{
    public class TemplatedLink
    {
        private readonly UriTemplate _uriTemplate;

        /// <summary>
        /// Create a new link
        /// </summary>
        /// <param name="template">The link template in RFC 6570 format</param>
        public TemplatedLink(string template)
        {
            _uriTemplate = new UriTemplate(template);
        }

        private static readonly Regex UriSplit = new Regex(@"(?<left>.*)(?<right>\{\?.*)|(?<left>.*)");

        /// <summary>
        /// Get the path part of the template. Useful for building routes etc
        /// </summary>
        /// <returns></returns>
        public string GetPathTemplate()
        {
            var matches = UriSplit.Match(_uriTemplate.Template);
            return matches.Groups["left"].Value;
        }

        /// <summary>
        /// Get the complete template as specified in the link
        /// </summary>
        /// <returns></returns>
        public UriTemplate GetTemplate()
        {
            return _uriTemplate;
        }

    }
}