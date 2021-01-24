using Rebus.Config;
using Rebus.Serialization.Custom;
using System;
using System.Collections.Generic;

namespace Rebus.Extensions.SharedNothing
{
    public static class CustomMessageTypeNamesExtensions
    {
        public static RebusConfigurer UseCustomMessageTypeNames(this RebusConfigurer rebus, Action<CustomTypeNameConventionBuilder> builder) => rebus
            .Serialization(s => builder(s.UseCustomMessageTypeNames()));
    }
}
