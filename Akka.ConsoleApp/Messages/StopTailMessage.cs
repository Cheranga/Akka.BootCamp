namespace Akka.ConsoleApp.Messages
{
    public class StopTailMessage
    {
        public string FilePath { get; }

        public StopTailMessage(string filePath)
        {
            FilePath = filePath;
        }
    }
}