using System;
using Xunit;

namespace Hyperspec.Tests.Links
{
    public class link_with_templated_params : TestBase
    {
        protected TemplatedLink TemplatedLink;
        protected override void Given()
        {
            TemplatedLink = new TemplatedLink("/test/{testString}/something/{testId}");
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
            Assert.Equal("/test/{testString}/something/{testId}", TemplatedLink.GetTemplate().Template);
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