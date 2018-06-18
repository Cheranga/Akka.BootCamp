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
            //var validationActor = actorSystem.ActorOf(Props.Create(() => new ValidationActor(consoleWriterActor)), "ValidationActor");
            var tailCoordinatorActor = actorSystem.ActorOf(Props.Create(()=>new TailCoordinatorActor()), "TailCoordinatorActor");
            var fileValidationActor = actorSystem.ActorOf(Props.Create(() => new FileValidatorActor(tailCoordinatorActor, consoleWriterActor)));
            var consoleReaderActor = actorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(fileValidationActor)), "ConsoleReaderActor");

            consoleReaderActor.Tell("start");

            actorSystem.WhenTerminated.Wait();
        }
    }
}