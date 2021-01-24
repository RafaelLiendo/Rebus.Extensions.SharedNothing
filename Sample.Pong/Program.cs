using Microsoft.Extensions.DependencyInjection;
using Rebus.ServiceProvider;
using Rebus.Extensions.SharedNothing;
using Rebus.Config;

namespace Sample.Pong
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputQueueName = "PongApp";
            var connectionString = "amqp://guest:guest@localhost:5672";

            var services = new ServiceCollection();
            services.AutoRegisterHandlersFromAssemblyOf<Program>();
            services.AddSingleton<Producer>();

            // 1.1. Configure Rebus
            services.AddRebus(configure => configure
                .Transport(t => t.UseRabbitMq(connectionString, inputQueueName))
                .Logging(l => l.None())
                .UseSharedNothingApproach(builder => builder
                    .AddWithCustomName<PingEvent2>("PingApp:PingEvent")
                    .AddWithCustomName<PongEvent2>("PongApp:PongEvent")
                    //optional
                    //.AllowFallbackToDefaultConvention()
                )
            );

            using (var provider = services.BuildServiceProvider())
            {
                provider.UseRebus(rebus =>
                {
                    rebus.Subscribe<PingEvent2>();
                    //or
                    //rebus.Advanced.Topics.Subscribe("PingApp:PingEvent");
                });

                var producer = provider.GetRequiredService<Producer>();
                producer.Produce();
            }
        }
    }
}
