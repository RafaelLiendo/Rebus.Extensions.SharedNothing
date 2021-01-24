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
