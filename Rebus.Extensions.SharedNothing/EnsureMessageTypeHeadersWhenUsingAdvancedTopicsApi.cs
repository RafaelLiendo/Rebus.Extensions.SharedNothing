using Rebus.Config;
using Rebus.Pipeline;
using System;
using System.Threading.Tasks;
using Rebus.Pipeline.Send;
using Rebus.Messages;
using System.Linq;

namespace RebusExtensions
{
    public static class EnsureMessageTypeHeadersWhenUsingAdvancedTopicsApiExtensions
    {
        public static OptionsConfigurer UseTopicNameAsMessageTypePipeline(this OptionsConfigurer c)
        {
            c.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();
                var step = new TopicNameAsMessageTypePipeline();
                return new PipelineStepInjector(pipeline)
                    .OnSend(step, PipelineRelativePosition.Before, typeof(AssignDefaultHeadersStep));
            });

            return c;
        }
    }

    [StepDocumentation(
@"When using bus.Advanced.Topics.Publish('Topic:Name', <AnonymousType>)
or bus.Advanced.Topics.Publish('Topic:Name', <UnmappedType>)
Set MessageType Headers from topic name")]
    public class TopicNameAsMessageTypePipeline : IOutgoingStep
    {
        public async Task Process(OutgoingStepContext context, Func<Task> next)
        {
            var message = context.Load<Message>();
            var headers = message.Headers;

            //if this is is a Publish() and rbs2-msg-type has not been set yet
            if (headers[Headers.Intent] == "pub" && !headers.ContainsKey(Headers.Type))
            {                
                var address = context.Load<DestinationAddresses>().FirstOrDefault();
                var separatorIndex = address.IndexOf('@');
                if (separatorIndex > 0)
                {
                    var topicName =  address.Substring(0, separatorIndex);
                    headers[Headers.Type] = topicName;
                }
            }

            await next();
        }
    }
}