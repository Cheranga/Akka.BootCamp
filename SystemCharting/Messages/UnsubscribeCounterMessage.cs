using Akka.Actor;

namespace SystemCharting.Messages
{
    public class UnsubscribeCounterMessage
    {
        public CounterType CounterType { get; }
        public IActorRef Subscriber { get; }

        public UnsubscribeCounterMessage(CounterType counterType, IActorRef subscriber)
        {
            CounterType = counterType;
            Subscriber = subscriber;
        }
    }
}