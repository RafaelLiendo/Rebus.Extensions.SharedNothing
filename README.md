# Rebus.Extensions.SharedNothing

Provides extensions to make to the "No code sharing" scenario easier to configure on [Rebus](https://github.com/rebus-org/Rebus).
Also it aims to introduce no breaking changes and even be able to have both scenarios at the same time, with or without sharing a dll.

![](https://raw.githubusercontent.com/rebus-org/Rebus/master/artwork/little_rebusbus2_copy-200x200.png)

---

## Motivation

See this issue https://github.com/rebus-org/Rebus/issues/852

## How is it better than the ["SharedNothing"](https://github.com/rebus-org/RebusSamples/tree/master/SharedNothing) sample provided?

The sample providade still depends on the fully quallified type name (including the assembly) to be able to deserialize the messages, as you can see here:

https://github.com/rebus-org/RebusSamples/blob/71f50f3d605cf23402e37057ab7f199d437b26f6/SharedNothing/Subscriber/CustomMessageDeserializer.cs#L22

### On the other hand, with this repo's implementation, the applications only have to agree on a topic name

## Show me the code!

### here is the first app

```csharp
using Microsoft.Extensions.DependencyInjection;
using Rebus.ServiceProvider;
using Rebus.Extensions.SharedNothing;
using Rebus.Config;

namespace Sample.Ping
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputQueueName = "PingApp";
            var connectionString = "amqp://guest:guest@localhost:5672";

            var services = new ServiceCollection();
            services.AutoRegisterHandlersFromAssemblyOf<Program>();

            services.AddSingleton<Producer>();

            services.AddRebus(configure => configure
                .Transport(t => t.UseRabbitMq(connectionString, inputQueueName))
                .Logging(l => l.None())
                .UseSharedNothingApproach(builder => builder
                    .AddWithCustomName<PingEvent1>("PingApp:PingEvent")
                    .AddWithCustomName<PongEvent1>("PongApp:PongEvent")
                    //optional
                    //.AllowFallbackToDefaultConvention()
                )
            );

            using (var provider = services.BuildServiceProvider())
            {
                provider.UseRebus(rebus =>
                {
                    rebus.Subscribe<PongEvent1>();
                    //or
                    //rebus.Advanced.Topics.Subscribe("PongApp:PongEvent");
                });

                var producer = provider.GetRequiredService<Producer>();
                producer.Produce();
            }
        }
    }
}

```

### You can publish messages relying on the type mapping showed above or you can explicitly specify the topic name

```csharp
_bus.Publish(new PingEvent1 { Foo = "FooValue" }).Wait();
//or
_bus.Advanced.Topics.Publish("PingApp:PingEvent", new { Foo = "FooValue" }).Wait();
//notice the anonymous type being used with no problem
```

### Same thing on the second app, the topics are the same, but they map to a completly different type

```csharp
namespace Sample.Pong
{
  class Program
  {

    var inputQueueName = "PongApp";

    ...

    .UseSharedNothingApproach(builder => builder
        .AddWithCustomName<PingEvent2>("PingApp:PingEvent")
        .AddWithCustomName<PongEvent2>("PongApp:PongEvent")
        //optional
        //.AllowFallbackToDefaultConvention()
    )

    ...

    rebus.Subscribe<PingEvent2>();
    //or
    //rebus.Advanced.Topics.Subscribe("PingApp:PingEvent");
  }
}
```

### Everything works as you would expect in a Rebus application

```csharp
using System;
using System.Threading.Tasks;
using Rebus.Handlers;

namespace Sample.Pong
{
    public class PingHandler : IHandleMessages<PingEvent2>
    {
        public Task Handle(PingEvent2 message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"PingHandler received : PingEvent2 {message.Foo}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
```

## How was that accomplished?

Whell... the it wasen't perfect. In an ideal solution you wouldn't require any headers to deserialize the message on the consumer side. But here we had to add the Topic name as the MessageType.

![](./Prints/Message.png)

#### But besides that, it the end result looks good. The queues and topics have the correct names.

![](./Prints/Queue%20and%20Exchange%20names.png)

#### And the consumer correctly desserialzies the message

![](./Prints/Console.png)
