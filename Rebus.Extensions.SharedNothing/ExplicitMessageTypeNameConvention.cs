using Rebus.Config;
using Rebus.Serialization.Custom;
using System;
using System.Collections.Generic;

namespace Rebus.Extensions.SharedNothing
{
    public static class ExplicitMessageTypeNameConventionExtensions
    {
        public static RebusConfigurer UseExplicitMessageTypeNameConvention(this RebusConfigurer rebus, Dictionary<string, Type> topicsDictionary) => rebus
            .Serialization(s => s
                .UseCustomMessageTypeNames()
                .ApplyCustomNames(topicsDictionary)
            );

        public static CustomTypeNameConventionBuilder ApplyCustomNames(this CustomTypeNameConventionBuilder builder, Dictionary<string, Type> topicsDictionary)
        {
            foreach (var (topic, type) in topicsDictionary)
            {
                builder.AddWithCustomName(type, topic);
            }

            return builder;
        }
    }
}
