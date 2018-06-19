namespace Akka.ConsoleApp.Messages
{
    public class FileErrorMessage
    {
        public string FileName { get; }
        public string Reason { get; }

        public FileErrorMessage(string fileName, string reason)
        {
            FileName = fileName;
            Reason = reason;
        }
    }
}