using System;
using Akka.Actor;
using Akka.ConsoleApp.Messages;

namespace Akka.ConsoleApp.Actors
{
    public class ConsoleReaderActor : UntypedActor
    {
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

            //
            // Get the validation actor using actor selection
            //
            var validatorActor = Context.ActorSelection("akka://MyActorSystem/user/FileValidatorActor");
            validatorActor.Tell(input);
        }
    }
}