using System;
using Xunit;

namespace Hyperspec.Tests.Links
{
    public class resource_form_with_no_template : TestBase
    {
        protected IForm ResourceForm;
        protected string LinkTemplate;

        protected override void Given()
        {
            LinkTemplate = "/test/{testString}/something/{testId}";
            var testObj = new MyExtendedTestClass()
            {
                TestString = "ATestString",
                TestId = new Guid("8babe164-fa02-47f0-b4e5-92bdc972ff01"),
                ExtraInt = 1337,
                ExtraString = "AnExtraString"
            };
            ResourceForm = new ResourceForm(LinkTemplate, new[] { new ContentContext(testObj) });
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_href_is_correct()
        {
            Assert.Equal("/test/ATestString/something/8babe164-fa02-47f0-b4e5-92bdc972ff01", ResourceForm.Href);
        }

        [Fact]
        public void then_form_parameters_are_correct()
        {
            var formParams = ResourceForm.Template;

            Assert.Equal(0, formParams.Count);
        }
    }
}