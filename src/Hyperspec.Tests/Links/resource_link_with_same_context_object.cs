using System;
using Xunit;

namespace Hyperspec.Tests.Links
{

    public class resource_link_with_same_context_object_and_null_value_in_query : TestBase
    {
        protected ILink Link;
        protected TemplatedLink TemplatedLink;
        protected override void Given()
        {
            TemplatedLink = new TemplatedLink("/test/{testString}/something/{testId}{?extraString,extraInt}");
            var testObj = new MyExtendedTestClass()
            {
                TestString = "ATestString",
                TestId = new Guid("8babe164-fa02-47f0-b4e5-92bdc972ff01"),
                ExtraInt = 1337,
                ExtraString = null
            };
            Link = new ResourceLink<MyExtendedTestClass>(TemplatedLink, new[] { testObj });
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

   
    public class resource_link_with_same_context_object : TestBase
    {
        protected ILink Link;
        protected TemplatedLink TemplatedLink;
        protected override void Given()
        {
            TemplatedLink = new TemplatedLink("/test/{testString}/something/{testId}{?extraString,extraInt}");
            var testObj = new MyExtendedTestClass()
            {
                TestString = "ATestString",
                TestId = new Guid("8babe164-fa02-47f0-b4e5-92bdc972ff01"),
                ExtraInt = 1337,
                ExtraString = "AnExtraString"
            };
            Link = new ResourceLink<MyExtendedTestClass>(TemplatedLink, new[] { testObj });
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/ATestString/something/8babe164-fa02-47f0-b4e5-92bdc972ff01?extraString=AnExtraString&extraInt=1337", Link.Href);
        }
    }
}