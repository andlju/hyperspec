using System;
using System.Collections.Generic;

namespace Hyperspec
{

    public static class HyperSpecConfiguration
    {
        public static Func<string> BaseLinkFactory = () => "";

        internal static Func<IEnumerable<object>, LinkBuilder> LinkBuilderFactory = (contexts) => new LinkBuilder(contexts, BaseLinkFactory());
    }
}