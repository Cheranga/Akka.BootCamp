namespace Akka.ConsoleApp.Messages
{
    public class NullInputMessage : InputErrorMessage
    {
        public NullInputMessage() : base("Input cannot be empty")
        {
        }
    }
}