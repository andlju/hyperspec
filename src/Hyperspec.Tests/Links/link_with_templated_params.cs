using System;
using Xunit;

namespace Hyperspec.Tests.Links
{
    public class link_with_templated_params : TestBase
    {
        protected Link Link;
        protected override void Given()
        {
            Link = new Link("/test/{testString}/something/{testId}");
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
            Assert.Equal("/test/{testString}/something/{testId}", Link.GetTemplate().Template);
        }
    }

    public class MyTestClass
    {
        public string TestString { get; set; }
        public Guid TestId { get; set; }
    }

    public class MyExtendedTestClass
    {
        public string TestString { get; set; }
        public Guid TestId { get; set; }
        public string ExtraString { get; set; }
        public int ExtraInt { get; set; }
    }

}