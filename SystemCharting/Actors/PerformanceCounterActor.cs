using System;
using System.Collections.Generic;
using System.Diagnostics;
using SystemCharting.Messages;
using Akka.Actor;
using Akka.Util.Internal;

namespace SystemCharting.Actors
{
    public class PerformanceCounterActor : ReceiveActor
    {
        private readonly Func<PerformanceCounter> _performanceCounterGenerator;
        private readonly string _seriesName;
        private readonly HashSet<IActorRef> _subscriptions;
        private ICancelable _cancelable;
        private PerformanceCounter _performanceCounter;

        public PerformanceCounterActor(string seriesName, Func<PerformanceCounter> performanceCounterGenerator)
        {
            _seriesName = seriesName;
            _performanceCounterGenerator = performanceCounterGenerator;
            _subscriptions = new HashSet<IActorRef>();

            Receive<GatherMetricsMessage>(message => HandleGatherMetricsMessage(message));
            Receive<SubscribeCounterMessage>(message => HandleSubscribeCounterMessage(message));
            Receive<UnsubscribeCounterMessage>(message => HandleUnsubscribeCounterMessage(message));
        }

        private void HandleGatherMetricsMessage(GatherMetricsMessage message)
        {
            //
            // Gather the next metric value and send the message to the subscribers
            //
            var msg = new MetricMessage(_seriesName,_performanceCounter.NextValue());
            _subscriptions.ForEach(x=>x.Tell(msg));
        }

        private void HandleSubscribeCounterMessage(SubscribeCounterMessage message)
        {
            _subscriptions.Add(message.Subscriber);
        }

        private void HandleUnsubscribeCounterMessage(UnsubscribeCounterMessage message)
        {
            _subscriptions.Remove(message.Subscriber);
        }

        protected override void PreStart()
        {
            //
            // Set the performance counter using the factory method passed
            //
            _performanceCounter = _performanceCounterGenerator();
            //
            // Set the cancelable so the operation can be cancelled when the actor stops
            //
            _cancelable = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromMilliseconds(250),
                TimeSpan.FromMilliseconds(250),
                Self,
                new GatherMetricsMessage(),
                Self);
        }

        protected override void PostStop()
        {
            try
            {
                _cancelable.Cancel(false);
                _performanceCounter.Dispose();
            }
            catch (Exception exception)
            {
                // ignored
            }
            finally
            {
                base.PostStop();
            }
        }
    }
}