namespace SystemCharting.Messages
{
    public class WatchMessge
    {
        public CounterType CounterType { get; }

        public WatchMessge(CounterType counterType)
        {
            CounterType = counterType;
        }
    }

    public class UnwatchMessage
    {
        public CounterType CounterType { get; }

        public UnwatchMessage(CounterType counterType)
        {
            CounterType = counterType;
        }
    }
}