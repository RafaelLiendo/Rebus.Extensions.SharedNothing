using Rebus.Config;
using Rebus.Serialization.Custom;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rebus.Extensions.SharedNothing
{
    public class TypesMarkedForSubscriptionSet : HashSet<Type>
    {
    }

    public static class CustomTypeNameConventionBuilderExtensions
    {
        public static RebusConfigurer UseTypesMarkedForSubscriptionExtension(this RebusConfigurer rebus, Action<CustomTypeNameConventionBuilder> builder) => rebus
            .Serialization(s => s
                .OtherService<TypesMarkedForSubscriptionSet>()
                .Register(c => new TypesMarkedForSubscriptionSet()));
    }
}
