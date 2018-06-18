namespace Akka.ConsoleApp.Messages
{
    public class InputErrorMessage
    {
        public string Reason { get; }

        public InputErrorMessage(string reason)
        {
            Reason = reason;
        }
    }
}