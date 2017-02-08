using Xunit;

namespace Hyperspec.Tests.Links
{
    public class resource_form_with_context_object_with_readonly_property : TestBase
    {
        protected IForm ResourceForm;
        protected string LinkTemplate;

        protected override void Given()
        {
            LinkTemplate = "/test";
            var testObj = new MyTestClassWithReadOnlyProperty()
            {
                TestString = "ATestString"
            };
            ResourceForm = new ResourceForm<MyTestClassWithReadOnlyProperty>(LinkTemplate, new[] { new ContentContext(testObj) });
        }

        protected override void When()
        {
            
        }

        [Fact]
        public void then_form_parameters_are_correct()
        {
            var formParams = ResourceForm.Template;
            Assert.Equal(1, formParams.Count);
            Assert.Equal("ATestString", formParams["TestString"].DefaultValue);
        }

    }
}