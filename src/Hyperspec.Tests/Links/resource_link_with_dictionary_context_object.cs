using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Hyperspec.Tests.Links
{
    public class resource_link_with_dictionary_context_object : TestBase
    {
        protected ILink Link;
        protected string LinkTemplate;
        protected override void Given()
        {
            LinkTemplate = "/test/{testString}/something/{testId}{?extraString,extraInt}";
            var testObj = new MyTestClass()
            {
                TestString = "ATestString",
                TestId = new Guid("8babe164-fa02-47f0-b4e5-92bdc972ff01")
            };
            var testDict = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            testDict["ExtraString"] = "MyExtraString";
            Link = new ResourceLink<MyExtendedTestClass>(LinkTemplate, new [] { new ContentContext(testObj), new ContentContext(testDict) });
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/ATestString/something/8babe164-fa02-47f0-b4e5-92bdc972ff01?extraString=MyExtraString{&extraInt}", Link.Href);
        }
    }
}