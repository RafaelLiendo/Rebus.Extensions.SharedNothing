using Rebus.Config;
using Rebus.Serialization.Json;

namespace Rebus.Extensions.SharedNothing
{
    static class PureJsonSerializationExtensions
    {
        public static RebusConfigurer UsePureJsonSerialization(this RebusConfigurer rebus) => rebus
            .Serialization(s => s.UseNewtonsoftJson(JsonInteroperabilityMode.PureJson));
    }
}
