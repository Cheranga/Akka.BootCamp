using System;
using System.IO;
using Akka.Actor;
using Akka.ConsoleApp.Messages;

namespace Akka.ConsoleApp.Utils
{
    public class FileOberserver : IDisposable
    {
        private readonly string _absoluteFilePath;
        private readonly IActorRef _tailActor;
        private readonly string _fileDirectory;
        private readonly string _fileName;
        private FileSystemWatcher _fileWatcher;

        public FileOberserver(string absoluteFilePath, IActorRef tailActor)
        {
            _absoluteFilePath = absoluteFilePath;
            _tailActor = tailActor;
            _fileDirectory = Path.GetDirectoryName(absoluteFilePath);
            _fileName = Path.GetFileName(absoluteFilePath);
        }

        public void Dispose()
        {
            _fileWatcher.Dispose();
        }

        public void Start()
        {
            _fileWatcher = new FileSystemWatcher(_fileDirectory, _fileName);
            _fileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

            _fileWatcher.Changed += OnChanged;
            _fileWatcher.Error += OnError;

            _fileWatcher.EnableRaisingEvents = true;
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            var message = new FileErrorMessage(_fileName, e.GetException().Message);
            _tailActor.Tell(message, ActorRefs.NoSender);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                var message = new FileWriteMessage(_fileName);
                _tailActor.Tell(message, ActorRefs.NoSender);
            }
        }
    }
}