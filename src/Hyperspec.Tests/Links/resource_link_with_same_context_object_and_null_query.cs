using Xunit;

namespace Hyperspec.Tests.Links
{
    public class resource_link_with_same_context_object_and_null_query : TestBase
    {
        protected ILink Link;
        protected string LinkTemplate;
        protected override void Given()
        {
            LinkTemplate = "/test/{testString}/something/{testId}";

            Link = new ResourceLink<MyExtendedTestClass>(LinkTemplate, new IContentContext[] {}, "test");
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/{testString}/something/{testId}{?ExtraString,ExtraInt}", Link.Href);
        }
    }
}