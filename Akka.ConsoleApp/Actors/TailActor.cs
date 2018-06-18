using Akka.Actor;

namespace Akka.ConsoleApp.Actors
{
    public class TailActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            HandleMessage(message);
        }

        private void HandleMessage(object message)
        {
            
        }
    }
}