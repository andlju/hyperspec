using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Hyperspec.Tests
{
    public abstract class TestBase
    {
        protected TestBase()
        {
            Given();
            When();
        }

        protected abstract void Given();

        protected abstract void When();
    }
}
