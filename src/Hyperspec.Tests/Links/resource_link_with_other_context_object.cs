using System;
using Xunit;

namespace Hyperspec.Tests.Links
{
    public class resource_link_with_specified_templateparameterinfo : TestBase
    {
        protected ILink Link;
        protected string LinkTemplate;
        protected override void Given()
        {
            LinkTemplate = "/test/{testString}/something/{testId}";
            var testObj = new MyTestClass()
            {
                TestString = "ATestString",
                TestId = new Guid("8babe164-fa02-47f0-b4e5-92bdc972ff01")
            };
            Link = new ResourceLink(LinkTemplate, new[] { new ContentContext(testObj), }, parameterInfos: new []
            {
                new ResourceLinkBase.TemplateParameterInfo()
                {
                    Name = "extraString"
                },
                new ResourceLinkBase.TemplateParameterInfo()
                {
                    Name = "extraInt"
                }, 
            });
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/ATestString/something/8babe164-fa02-47f0-b4e5-92bdc972ff01{?extraString,extraInt}", Link.Href);
        }
    }

    public class resource_link_with_other_context_object : TestBase
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
            Link = new ResourceLink<MyExtendedTestClass>(LinkTemplate, new[] { new ContentContext(testObj),  });
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/ATestString/something/8babe164-fa02-47f0-b4e5-92bdc972ff01{?extraString,extraInt}", Link.Href);
        }
    }
}