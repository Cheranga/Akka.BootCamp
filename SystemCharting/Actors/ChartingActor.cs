using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SystemCharting.Messages;
using Akka.Actor;

namespace SystemCharting.Actors
{
    public class ChartingActor : ReceiveActor, IWithUnboundedStash
    {
        public const int MaxPoints = 250;
        private int xPosCounter = 0;

        private readonly Chart _chart;
        private readonly Button _pauseButton;
        private Dictionary<string, Series> _seriesIndex;

        public ChartingActor(Chart chart, Button pauseButton) : this(chart, new Dictionary<string, Series>(), pauseButton)
        {
        }

        public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex, Button pauseButton)
        {
            _chart = chart;
            _seriesIndex = seriesIndex;
            _pauseButton = pauseButton;

            Receiving();
        }

        private void Receiving()
        {
            Receive<InitializeChartMessage>(message => HandleInitialize(message));
            Receive<AddSeriesMessage>(message => HandleAddSeries(message));
            Receive<RemoveSeriesMessage>(removeSeries => HandleRemoveSeries(removeSeries));
            Receive<MetricMessage>(metric => HandleMetrics(metric));

            Receive<TogglePauseMessage>(message =>
            {
                SetPauseButtonText(true);
                BecomeStacked(Paused);
            });
        }

        private void SetPauseButtonText(bool paused)
        {
            _pauseButton.Text = $"{(paused ? "Play |>" : "Pause ||")}";
        }

        private void Paused()
        {
            Receive<AddSeriesMessage>(messge => Stash.Stash());
            Receive<RemoveSeriesMessage>(message => Stash.Stash());
            Receive<MetricMessage>(message => HandleMetricsPaused(message));

            Receive<TogglePauseMessage>(message =>
            {
                SetPauseButtonText(false);
                UnbecomeStacked();
                
                Stash.UnstashAll();
            });
        }

        private void HandleMetricsPaused(MetricMessage message)
        {
            if (!string.IsNullOrEmpty(message.Series) && _seriesIndex.ContainsKey(message.Series))
            {
                var series = _seriesIndex[message.Series];
                if (series.Points == null)
                {
                    return; // means we're shutting down
                }
                series.Points.AddXY(xPosCounter++, 0.0d); //set the Y value to zero when we're paused

                while (series.Points.Count > MaxPoints)
                {
                    series.Points.RemoveAt(0);
                }

                SetChartBoundaries();
            }
        }

        private void SetChartBoundaries()
        {
            double maxAxisX, maxAxisY, minAxisX, minAxisY = 0.0d;
            var allPoints = _seriesIndex.Values.SelectMany(series => series.Points).ToList();
            var yValues = allPoints.SelectMany(point => point.YValues).ToList();
            maxAxisX = xPosCounter;
            minAxisX = xPosCounter - MaxPoints;
            maxAxisY = yValues.Count > 0 ? Math.Ceiling(yValues.Max()) : 1.0d;
            minAxisY = yValues.Count > 0 ? Math.Floor(yValues.Min()) : 0.0d;
            if (allPoints.Count > 2)
            {
                var area = _chart.ChartAreas[0];
                area.AxisX.Minimum = minAxisX;
                area.AxisX.Maximum = maxAxisX;
                area.AxisY.Minimum = minAxisY;
                area.AxisY.Maximum = maxAxisY;
            }
        }

        #region Individual Message Type Handlers

        private void HandleInitialize(InitializeChartMessage ic)
        {
            if (ic.InitialSeries != null)
            {
                // swap the two series out
                _seriesIndex = ic.InitialSeries;
            }

            // delete any existing series
            _chart.Series.Clear();

            // set the axes up
            var area = _chart.ChartAreas[0];
            area.AxisX.IntervalType = DateTimeIntervalType.Number;
            area.AxisY.IntervalType = DateTimeIntervalType.Number;

            SetChartBoundaries();

            // attempt to render the initial chart
            if (_seriesIndex.Any())
            {
                foreach (var series in _seriesIndex)
                {
                    // force both the chart and the internal index to use the same names
                    series.Value.Name = series.Key;
                    _chart.Series.Add(series.Value);
                }
            }

            SetChartBoundaries();
        }

        private void HandleAddSeries(AddSeriesMessage series)
        {
            if (!string.IsNullOrEmpty(series.Series.Name) &&
                !_seriesIndex.ContainsKey(series.Series.Name))
            {
                _seriesIndex.Add(series.Series.Name, series.Series);
                _chart.Series.Add(series.Series);
                SetChartBoundaries();
            }
        }

        private void HandleRemoveSeries(RemoveSeriesMessage series)
        {
            if (!string.IsNullOrEmpty(series.SeriesName) &&
                _seriesIndex.ContainsKey(series.SeriesName))
            {
                var seriesToRemove = _seriesIndex[series.SeriesName];
                _seriesIndex.Remove(series.SeriesName);
                _chart.Series.Remove(seriesToRemove);
                SetChartBoundaries();
            }
        }

        private void HandleMetrics(MetricMessage metric)
        {
            if (!string.IsNullOrEmpty(metric.Series) &&
                _seriesIndex.ContainsKey(metric.Series))
            {
                var series = _seriesIndex[metric.Series];
                series.Points.AddXY(xPosCounter++, metric.CounterValue);
                while (series.Points.Count > MaxPoints) series.Points.RemoveAt(0);
                SetChartBoundaries();
            }
        }

        #endregion

        public IStash Stash { get; set; }
    }
}