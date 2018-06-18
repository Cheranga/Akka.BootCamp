using Akka.Actor;
using Akka.ConsoleApp.Actors;

namespace Akka.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("MyActorSystem");

            var consoleWriterActor = actorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()), "ConsoleWriterActor");
            var validationActor = actorSystem.ActorOf(Props.Create(() => new ValidationActor(consoleWriterActor)), "ValidationActor");
            var consoleReaderActor = actorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(validationActor)), "ConsoleReaderActor");

            consoleReaderActor.Tell("start");

            actorSystem.WhenTerminated.Wait();
        }
    }
}