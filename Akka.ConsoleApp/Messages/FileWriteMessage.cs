using System.CodeDom;

namespace Akka.ConsoleApp.Messages
{
    public class FileWriteMessage
    {
        public string FileName { get; }

        public FileWriteMessage(string fileName)
        {
            FileName = fileName;
        }
    }
}