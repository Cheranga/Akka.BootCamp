using Akka.Actor;
using Akka.ConsoleApp.Messages;

namespace Akka.ConsoleApp.Actors
{
    public class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
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
                _consoleWriterActor.Tell(messageToSend);
            }
            else
            {
                var isValid = msg.Length % 2 == 0;
                if (isValid)
                {
                    _consoleWriterActor.Tell(new ValidInputMessage($"{msg} is valid!"));
                }
                else
                {
                    _consoleWriterActor.Tell(new InvalidInputMessage($"{msg} is invalid!"));
                }
            }

            Sender.Tell(new ContinueProcessingMessage());
        }
    }
}