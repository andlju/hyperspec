using System;
using System.Collections.Generic;
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
            Link = new ResourceLink(LinkTemplate, new[] { new ContentContext(testObj), }, "test", parameterInfos: new []
            {
                new TemplateParameterInfo()
                {
                    Name = "extraString"
                },
                new TemplateParameterInfo()
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

    public class resource_link_with_template_and_specified_templateparameterinfo : TestBase
    {
        protected ILink Link;
        protected string LinkTemplate;
        protected override void Given()
        {
            LinkTemplate = "/test/{testString}/something/{testId}";
            var testObj = new MyExtendedTestClass()
            {
                TestString = "ATestString",
                TestId = new Guid("8babe164-fa02-47f0-b4e5-92bdc972ff01"),
                ExtraInt = 10,
                ExtraString="test"
            };
            Link = new ResourceLink<MyExtendedTestClass>(LinkTemplate, new[] { new ContentContext(testObj), }, "test", true, parameterInfos: new[]
            {
                new TemplateParameterInfo()
                {
                    Name = "ExtraString",
                    ForceTemplated = true
                },
                new TemplateParameterInfo()
                {
                    Name = "extraStringWithDefault",
                    ForceTemplated = true
                },
                new TemplateParameterInfo()
                {
                    Name = "extraIntWithDefault",
                    ForceTemplated = true
                },
            });
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/ATestString/something/8babe164-fa02-47f0-b4e5-92bdc972ff01?ExtraInt=10{&ExtraString,extraStringWithDefault,extraIntWithDefault}", Link.Href);
        }

        [Fact]
        public void then_templated_parameters_are_in_template()
        {
            Assert.True(Link.Template.ContainsKey("extraStringWithDefault"));
            Assert.True(Link.Template.ContainsKey("extraIntWithDefault"));
            Assert.True(Link.Template.ContainsKey("ExtraString"));
        }

        [Fact]
        public void then_default_values_are_correct()
        {
            Assert.Equal("test", Link.Template["ExtraString"].DefaultValue);
        }
    }

}