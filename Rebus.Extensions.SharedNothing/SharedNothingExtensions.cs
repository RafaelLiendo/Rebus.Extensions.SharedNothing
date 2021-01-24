using Rebus.Config;
using Rebus.Serialization.Custom;
using System;

namespace Rebus.Extensions.SharedNothing
{
    public static class SharedNothingExtensions
    {
        public static RebusConfigurer UseSharedNothingApproach(this RebusConfigurer rebus, Action<CustomTypeNameConventionBuilder> builder) => rebus
            .UsePureJsonSerialization()
            .UseTopicNameSameAsMessageTypeNameConvetion()
            .UseTopicNameAsMessageTypePipeline()
            .UseCustomMessageTypeNames(builder);
    }
}
