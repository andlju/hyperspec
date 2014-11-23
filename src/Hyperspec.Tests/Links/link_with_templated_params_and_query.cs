using Xunit;

namespace Hyperspec.Tests.Links
{
   
    public class link_with_templated_params_and_query : TestBase
    {
        protected TemplatedLink TemplatedLink;
        protected override void Given()
        {
            TemplatedLink = new TemplatedLink("/test/{testString}/something/{testId}{?extraString,extraInt}");
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_pathTemplate_is_correct()
        {
            Assert.Equal("/test/{testString}/something/{testId}", TemplatedLink.GetPathTemplate());
        }

        [Fact]
        public void then_fullTemplate_is_correct()
        {
            Assert.Equal("/test/{testString}/something/{testId}{?extraString,extraInt}", TemplatedLink.GetTemplate().Template);
        }
    }
}