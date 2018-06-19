using Akka.Actor;

namespace Akka.ConsoleApp.Messages
{
    public class StartTailMessage
    {
        public string FilePath { get; }
        public IActorRef ReporterActor { get; }

        public StartTailMessage(string filePath, IActorRef reporterActor)
        {
            FilePath = filePath;
            ReporterActor = reporterActor;
        }
    }
}