using Xunit;

namespace Hyperspec.Tests.Links
{
    public class link_with_no_params : TestBase
    {
        protected TemplatedLink TemplatedLink;
        protected override void Given()
        {
            TemplatedLink = new TemplatedLink("/test");
        }

        protected override void When()
        {
            
        }
        
        [Fact]
        public void then_path_template_is_correct()
        {
            
            Assert.Equal("/test", TemplatedLink.GetPathTemplate());
        }

    }
}