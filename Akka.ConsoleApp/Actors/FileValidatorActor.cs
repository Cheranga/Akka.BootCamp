using System;
using System.IO;
using Akka.Actor;
using Akka.ConsoleApp.Messages;

namespace Akka.ConsoleApp.Actors
{
    public class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef _consoleWriter;

        public FileValidatorActor(IActorRef consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        protected override void OnReceive(object message)
        {
            HandleMessage(message);
        }

        private void HandleMessage(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                var messageToSend = new NullInputMessage();
                _consoleWriter.Tell(messageToSend);

                Sender.Tell(new ContinueProcessingMessage());
            }
            else
            {
                var isValid = File.Exists(msg);
                if (isValid)
                {
                    _consoleWriter.Tell(new ValidInputMessage($"Starting processing for: {msg}"));
                    //
                    // Use actor selection to get the tail coordinator actor
                    //
                    var tailCoordinator = Context.ActorSelection("akka://MyActorSystem/user/TailCoordinatorActor");
                    tailCoordinator.Tell(new StartTailMessage(msg, _consoleWriter));
                }
                else
                {
                    _consoleWriter.Tell(new InvalidInputMessage("File does not exist"));
                    Sender.Tell(new ContinueProcessingMessage());
                }
            }
        }
    }
}