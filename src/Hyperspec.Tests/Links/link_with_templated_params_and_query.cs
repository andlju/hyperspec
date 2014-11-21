using Xunit;

namespace Hyperspec.Tests.Links
{
   
    public class link_with_templated_params_and_query : TestBase
    {
        protected Link Link;
        protected override void Given()
        {
            Link = new Link("/test/{testString}/something/{testId}{?extraString,extraInt}");
        }

        protected override void When()
        {

        }

        [Fact]
        public void then_pathTemplate_is_correct()
        {
            Assert.Equal("/test/{testString}/something/{testId}", Link.GetPathTemplate());
        }

        [Fact]
        public void then_fullTemplate_is_correct()
        {
            Assert.Equal("/test/{testString}/something/{testId}{?extraString,extraInt}", Link.GetTemplate().Template);
        }
    }
}