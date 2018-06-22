using System.IO;
using Akka.Actor;
using Akka.ConsoleApp.Messages;
using Akka.ConsoleApp.Utils;

namespace Akka.ConsoleApp.Actors
{
    public class TailActor : UntypedActor
    {
        private readonly string _filePath;
        private readonly IActorRef _reporterActor;
        private FileOberserver _fileObserver;
        private StreamReader _reader;
        private Stream _stream;

        public TailActor(string filePath, IActorRef reporterActor)
        {
            _filePath = filePath;
            _reporterActor = reporterActor;
        }

        protected override void OnReceive(object message)
        {
            HandleMessage(message);
        }

        private void HandleMessage(object message)
        {
            if (message is FileWriteMessage)
            {
                var content = _reader.ReadToEnd();
                if (!string.IsNullOrEmpty(content))
                {
                    _reporterActor.Tell(content);
                }
            }
            else if (message is FileErrorMessage)
            {
                _reporterActor.Tell($"Error:{((FileErrorMessage) message).Reason}");
            }
            else if (message is InitialReadMessage)
            {
                _reporterActor.Tell(((InitialReadMessage) message).Text);
            }
        }

        protected override void PreStart()
        {
            _fileObserver = new FileOberserver(_filePath, Self);
            _fileObserver.Start();

            _stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _reader = new StreamReader(_stream);

            var fileContent = _reader.ReadToEnd();
            var message = new InitialReadMessage(_filePath, fileContent);
            Self.Tell(message);
        }

        protected override void PostStop()
        {
            _fileObserver.Dispose();
            _fileObserver = null;

            _reader.Close();
            _reader.Dispose();

            _stream.Close();
            _stream.Dispose();

            base.PostStop();
        }
    }
}