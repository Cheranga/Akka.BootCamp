namespace Akka.ConsoleApp.Messages
{
    public class NullInputMessage : InputErrorMessage
    {
        public NullInputMessage(string reason) : base(reason)
        {
        }
    }
}