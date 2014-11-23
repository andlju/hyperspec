using System;
using Xunit;

namespace Hyperspec.Tests.Links
{
    public class resource_link_with_same_context_object_and_automatic_querystring : TestBase
    {
        protected ILink Link;
        protected string LinkTemplate;
        protected override void Given()
        {
            LinkTemplate = "/test/{testString}/something/{testId}{?extraString}";
            var testObj = new MyExtendedTestClass()
            {
                TestString = "ATestString",
                TestId = new Guid("8babe164-fa02-47f0-b4e5-92bdc972ff01"),
                ExtraInt = 1337,
                ExtraString = "AnExtraString"
            };
            Link = new ResourceLink<MyExtendedTestClass>(LinkTemplate, new[] { testObj });
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/ATestString/something/8babe164-fa02-47f0-b4e5-92bdc972ff01?extraString=AnExtraString&ExtraInt=1337", Link.Href);
        }
    }
}