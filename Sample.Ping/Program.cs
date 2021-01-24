using Microsoft.Extensions.DependencyInjection;
using Rebus.ServiceProvider;
using Rebus.Extensions.SharedNothing;
using System.Collections.Generic;
using System;
using Rebus.Config;
using Rebus.Serialization.Custom;

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
