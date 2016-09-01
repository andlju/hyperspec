using Xunit;

namespace Hyperspec.Tests.Links
{
    public class resource_link_with_no_context_and_no_template : TestBase
    {
        protected ILink Link;
        protected string LinkTemplate;
        protected override void Given()
        {
            LinkTemplate = "/test/something";
            Link = new ResourceLink(LinkTemplate, null);
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/something", Link.Href);
        }
    }
}