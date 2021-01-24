using Rebus.Config;
using Rebus.Serialization;
using Rebus.Topic;
using System;

namespace Rebus.Extensions.SharedNothing
{
    static class TopicNameSameAsMessageTypeNameConvetionExtensions
    {
        internal static RebusConfigurer UseTopicNameSameAsMessageTypeNameConvetion(this RebusConfigurer rebus) => rebus
            .Options(o =>
            {
                o.Register<ITopicNameConvention>(c => new TopicNameSameAsMessageTypeNameConvetion(c.Get<IMessageTypeNameConvention>()));
            });
    }    

    public class TopicNameSameAsMessageTypeNameConvetion : ITopicNameConvention
    {
        private readonly IMessageTypeNameConvention message_TypeNameConvention;

        public TopicNameSameAsMessageTypeNameConvetion(IMessageTypeNameConvention messageTypeNameConvention) => 
            message_TypeNameConvention = messageTypeNameConvention;
        public string GetTopic(Type type) => message_TypeNameConvention.GetTypeName(type);
    }
}
