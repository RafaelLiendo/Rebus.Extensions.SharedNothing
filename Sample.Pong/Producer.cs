
using System;
using Rebus.Bus;

namespace Sample.Pong
{
    public class Producer
    {
        private readonly IBus _bus;

        public Producer(IBus bus)
        {
            _bus = bus;
        }

        public void Produce()
        {
            var keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine(@"
a) Publish using type name convetion
b) Publish with explicit topic naming
q) Quit
");
                var key = char.ToLower(Console.ReadKey(true).KeyChar);

                switch (key)
                {
                    case 'a':
                        PublishUsingTypeNameConvetion();
                        break;
                    case 'b':
                        PublishWithExplicitTopicNaming();
                        break;
                    case 'q':
                        Console.WriteLine("Quitting");
                        keepRunning = false;
                        break;
                }
            }

            Console.WriteLine("Consumer listening - press ENTER to quit");
            Console.ReadLine();
        }

        void PublishUsingTypeNameConvetion()
        {
            Console.WriteLine("Publishing PongEvent2");
            _bus.Publish(new PongEvent2 { Bar = "BarValue" }).Wait();
        }

        void PublishWithExplicitTopicNaming()
        {
            Console.WriteLine("Publishing PongEvent2");
            _bus.Advanced.Topics.Publish("PongApp:PongEvent", new { Bar = "BarValue" }).Wait();
        }
    }
}
