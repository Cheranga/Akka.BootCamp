using System;
using Akka.Actor;
using Akka.ConsoleApp.Messages;

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

            if (string.IsNullOrEmpty(input))
            {
                var message = new NullInputMessage();
                _consoleWriterActor.Tell(message);
            }
            else
            {
                var isValid = input.Length % 2 == 0;
                if (isValid)
                {
                    _consoleWriterActor.Tell(new ValidInputMessage($"{input} is valid!"));
                }
                else
                {
                    _consoleWriterActor.Tell(new InvalidInputMessage($"{input} is invalid!"));
                }
            }

            Self.Tell(new ContinueProcessingMessage());
        }
    }
}