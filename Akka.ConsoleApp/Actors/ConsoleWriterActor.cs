﻿using System;
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
            Console.WriteLine(message);
        }
    }
}