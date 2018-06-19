namespace Akka.ConsoleApp.Messages
{
    public class InitialReadMessage
    {
        public string FileName { get; }
        public string Text { get; }

        public InitialReadMessage(string fileName, string text)
        {
            FileName = fileName;
            Text = text;
        }
    }
}