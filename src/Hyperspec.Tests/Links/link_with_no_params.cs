using Xunit;

namespace Hyperspec.Tests.Links
{
    public class link_with_no_params : TestBase
    {
        protected Link Link;
        protected override void Given()
        {
            Link = new Link("/test");
        }

        protected override void When()
        {
            
        }
        
        [Fact]
        public void then_path_template_is_correct()
        {
            
            Assert.Equal("/test", Link.GetPathTemplate());
        }

    }
}