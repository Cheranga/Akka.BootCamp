using System;
using Akka.Actor;

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
            var msg = message as string;

            if (string.IsNullOrEmpty(msg))
            {
                Console.WriteLine("Input is invalid...");
            }
            else
            {
                var isValid = msg.Length % 2 == 0;
                Console.WriteLine(isValid? "Valid!" : "Invalid!");
            }
        }
    }
}