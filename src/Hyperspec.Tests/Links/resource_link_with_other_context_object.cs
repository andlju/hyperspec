using System;
using Xunit;

namespace Hyperspec.Tests.Links
{
    public class resource_link_with_other_context_object : TestBase
    {
        protected IResourceLink ResourceLink;
        protected Link Link;
        protected override void Given()
        {
            Link = new Link("/test/{testString}/something/{testId}{?extraString,extraInt}");
            var testObj = new MyTestClass()
            {
                TestString = "ATestString",
                TestId = new Guid("8babe164-fa02-47f0-b4e5-92bdc972ff01")
            };
            ResourceLink = new ResourceLink<MyExtendedTestClass>(Link, new[] { testObj });
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/ATestString/something/8babe164-fa02-47f0-b4e5-92bdc972ff01{?extraString,extraInt}", ResourceLink.Href);
        }
    }
}