using System;
using Akka.Actor;

namespace Akka.ConsoleApp.Actors
{
    public class ConsoleReaderActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            HandleMessage();
        }

        private void HandleMessage()
        {
            var input = Console.ReadLine();

            if (string.Equals("exit", input, StringComparison.OrdinalIgnoreCase))
            {
                Context.Stop(Self);
                return;
            }

            _consoleWriterActor.Tell(input);

            Self.Tell("continue...");
        }
    }
}