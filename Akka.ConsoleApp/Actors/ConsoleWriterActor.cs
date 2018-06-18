using System;
using Akka.Actor;
using Akka.ConsoleApp.Messages;

namespace Akka.ConsoleApp.Actors
{
    public class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            HandleMessage(message);
        }

        private void HandleMessage(object message)
        {
            if (message is InvalidInputMessage)
            {
                Console.WriteLine((message as InvalidInputMessage).Reason);
            }
            else if(message is ValidInputMessage)
            {
                Console.WriteLine((message as ValidInputMessage).Reason);
            }
        }
    }
}