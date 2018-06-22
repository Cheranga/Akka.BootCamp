using Akka.Actor;

namespace SystemCharting.Messages
{
    public class SubscribeCounterMessage
    {
        public CounterType CounterType { get; }
        public IActorRef Subscriber { get; }

        public SubscribeCounterMessage(CounterType counterType, IActorRef subscriber)
        {
            CounterType = counterType;
            Subscriber = subscriber;
        }
    }
}