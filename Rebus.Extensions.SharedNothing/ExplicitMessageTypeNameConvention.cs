using Rebus.Config;
using Rebus.Serialization.Custom;
using System;
using System.Collections.Generic;

namespace Rebus.Extensions.SharedNothing
{
    static class ExplicitMessageTypeNameConventionExtensions
    {
        public static RebusConfigurer UseExplicitMessageTypeNameConvention(this RebusConfigurer rebus, Dictionary<string, Type> topicsDictionary, bool allowFallbackToDefaultConvention = false) => rebus
            .Serialization(s => s
                .UseCustomMessageTypeNames()
                .ApplyCustomNames(topicsDictionary, allowFallbackToDefaultConvention)
            );

        public static CustomTypeNameConventionBuilder ApplyCustomNames(this CustomTypeNameConventionBuilder builder, Dictionary<string, Type> topicsDictionary, bool allowFallbackToDefaultConvention)
        {
            foreach (var (topic, type) in topicsDictionary)
            {
                builder.AddWithCustomName(type, topic);
            }

            if (allowFallbackToDefaultConvention)
            {
                builder.AllowFallbackToDefaultConvention();
            }

            return builder;
        }
    }
}
