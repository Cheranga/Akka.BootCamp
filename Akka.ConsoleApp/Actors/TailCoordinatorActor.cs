using System;
using Akka.Actor;
using Akka.ConsoleApp.Messages;

namespace Akka.ConsoleApp.Actors
{
    public class TailCoordinatorActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            HandleMessage(message);
        }

        private void HandleMessage(object message)
        {
            if (message is StartTailMessage)
            {
                var msg = message as StartTailMessage;
                Context.ActorOf(Props.Create(() => new TailActor(msg.FilePath, msg.ReporterActor)));
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                10,
                30000,
                exception =>
                {
                    if (exception is ArithmeticException)
                    {
                        return Directive.Resume;
                    }

                    if (exception is NotSupportedException)
                    {
                        return Directive.Stop;
                    }

                    return Directive.Restart;
                });
        }
    }
}