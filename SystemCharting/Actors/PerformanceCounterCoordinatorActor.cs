using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using SystemCharting.Messages;
using Akka.Actor;

namespace SystemCharting.Actors
{
    public class PerformanceCounterCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef _chartingActor;
        private readonly Dictionary<CounterType, IActorRef> _counterActors;

        private readonly IDictionary<CounterType, Func<PerformanceCounter>> _counterGenerators =
            new Dictionary<CounterType, Func<PerformanceCounter>>
            {
                {CounterType.Cpu, () => new PerformanceCounter("Processor", "% Processor Time", "_Total", true)},
                {CounterType.Memory, () => new PerformanceCounter("Memory", "% Committed Bytes In Use", true)},
                {CounterType.Disk, () => new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total", true)}
            };

        private IDictionary<CounterType, Func<Series>> _counterSeries = new Dictionary<CounterType, Func<Series>>
        {
            {
                CounterType.Cpu, () =>
                    new Series(CounterType.Cpu.ToString())
                    {
                        ChartType = SeriesChartType.SplineArea, Color = Color.DarkGreen
                    }
            },
            {
                CounterType.Memory, () =>
                    new Series(CounterType.Memory.ToString())
                    {
                        ChartType = SeriesChartType.FastLine,
                        Color = Color.MediumBlue
                    }
            },
            {
                CounterType.Disk, () =>
                    new Series(CounterType.Disk.ToString())
                    {
                        ChartType = SeriesChartType.SplineArea,
                        Color = Color.DarkRed
                    }
            }
        };

        public PerformanceCounterCoordinatorActor(IActorRef chartingActor) : this(chartingActor, new Dictionary<CounterType, IActorRef>())
        {

        }

        public PerformanceCounterCoordinatorActor(IActorRef chartingActor, Dictionary<CounterType, IActorRef> counterActors)
        {
            _chartingActor = chartingActor;
            _counterActors = counterActors;

            Receive<WatchMessge>(message => WatchMessageHandler(message));

            Receive<UnwatchMessage>(message => UnwatchMessageHandler(message));
        }

        private void UnwatchMessageHandler(UnwatchMessage message)
        {
            if (!_counterActors.ContainsKey(message.CounterType))
            {
                return;
            }

            _counterActors[message.CounterType].Tell(new UnsubscribeCounterMessage(message.CounterType, _chartingActor));
            _chartingActor.Tell(new RemoveSeriesMessage(message.CounterType.ToString()));

        }

        private void WatchMessageHandler(WatchMessge message)
        {
            if (!_counterActors.ContainsKey(message.CounterType))
            {
                var counterActor = Context.ActorOf(
                    Props.Create(() => new PerformanceCounterActor(message.CounterType.ToString(), _counterGenerators[message.CounterType])), $"{message.CounterType}PerformanceCounterActor");

                _counterActors.Add(message.CounterType, counterActor);
            }

            _chartingActor.Tell(new AddSeriesMessage(_counterSeries[message.CounterType]()));

            _counterActors[message.CounterType].Tell(new SubscribeCounterMessage(message.CounterType, _chartingActor));
        }
    }
}