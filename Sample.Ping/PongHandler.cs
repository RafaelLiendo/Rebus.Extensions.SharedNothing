using System;
using System.Threading.Tasks;
using Rebus.Handlers;

namespace Sample.Ping
{
    public class PongHandler : IHandleMessages<PongEvent1>
    {
        public Task Handle(PongEvent1 message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"PongHandler received : PongEvent1 {message.Bar}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
