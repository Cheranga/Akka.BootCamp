using System.Windows.Forms;
using SystemCharting.Messages;
using Akka.Actor;

namespace SystemCharting.Actors
{
    public class ButtonToggleActor : ReceiveActor
    {
        private readonly IActorRef _coordinatorActor;
        private readonly Button _myButton;
        private readonly CounterType _myCounterType;
        private bool _isToggledOn;

        public ButtonToggleActor(IActorRef coordinatorActor, Button myButton,
            CounterType myCounterType, bool isToggledOn = false)
        {
            _coordinatorActor = coordinatorActor;
            _myButton = myButton;
            _isToggledOn = isToggledOn;
            _myCounterType = myCounterType;

            Receive<ToggleMessage>(message => ToggleMessageHandler(message));
        }

        private void ToggleMessageHandler(ToggleMessage message)
        {
            if (_isToggledOn)
            {
                _coordinatorActor.Tell(new UnwatchMessage(_myCounterType));
            }
            else
            {
                _coordinatorActor.Tell(new WatchMessge(_myCounterType));
            }

            FlipToggle();
        }

        private void FlipToggle()
        {
            // flip the toggle
            _isToggledOn = !_isToggledOn;

            // change the text of the button
            _myButton.Text = string.Format("{0} ({1})",
                _myCounterType.ToString().ToUpperInvariant(),
                _isToggledOn ? "OFF" : "ON");
        }
    }
}