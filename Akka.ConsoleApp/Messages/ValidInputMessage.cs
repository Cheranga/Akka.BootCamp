namespace Akka.ConsoleApp.Messages
{
    public class ValidInputMessage
    {
        public string Reason { get; }

        public ValidInputMessage(string reason)
        {
            Reason = reason;
        }
    }
}