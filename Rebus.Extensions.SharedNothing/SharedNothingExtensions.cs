using Rebus.Config;
using System;
using System.Collections.Generic;

namespace Rebus.Extensions.SharedNothing
{
    public static class SharedNothingExtensions
    {
        public static RebusConfigurer UseSharedNothingApproach(this RebusConfigurer rebus) => rebus
            .UsePureJsonSerialization()
            .UseTopicNameSameAsMessageTypeNameConvetion()
            .UseTopicNameAsMessageTypePipeline();
    }
}
