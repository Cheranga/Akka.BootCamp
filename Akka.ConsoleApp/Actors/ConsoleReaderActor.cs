using System;
using Akka.Actor;
using Akka.ConsoleApp.Messages;

namespace Akka.ConsoleApp.Actors
{
    public class ConsoleReaderActor : UntypedActor
    {
        private readonly IActorRef _validationActor;

        public ConsoleReaderActor(IActorRef validationActor)
        {
            _validationActor = validationActor;
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

            _validationActor.Tell(input);
        }
    }
}