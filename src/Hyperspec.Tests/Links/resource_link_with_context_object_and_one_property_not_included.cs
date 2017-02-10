using System;
using Xunit;

namespace Hyperspec.Tests.Links
{
    public class resource_link_with_context_object_and_one_property_not_included : TestBase
    {
        protected ILink Link;
        protected string LinkTemplate;
        protected override void Given()
        {
            LinkTemplate = "/test/{testString}/something/{testId}{?extraString,extraInt}";
            var testObj = new MyExtendedTestClass()
            {
                TestString = "ATestString",
                TestId = new Guid("8babe164-fa02-47f0-b4e5-92bdc972ff01"),
                ExtraInt = 1337,
                ExtraString = "AnExtraString"
            };
            Link = new ResourceLink<MyExtendedTestClass>(LinkTemplate, new[] { new ContentContext(testObj, (name, val) => !name.Equals("ExtraString", StringComparison.InvariantCultureIgnoreCase)) }, "test");
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/ATestString/something/8babe164-fa02-47f0-b4e5-92bdc972ff01?extraInt=1337{&extraString}", Link.Href);
        }
    }
}