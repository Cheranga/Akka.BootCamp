namespace Akka.ConsoleApp.Messages
{
    public class InvalidInputMessage: InputErrorMessage
    {
        public InvalidInputMessage(string reason) : base(reason)
        {
        }
    }
}