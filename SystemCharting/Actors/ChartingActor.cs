using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using SystemCharting.Messages;
using Akka.Actor;

namespace SystemCharting.Actors
{
    public class ChartingActor : ReceiveActor
    {
        private readonly Chart _chart;
        private Dictionary<string, Series> _seriesIndex;

        public ChartingActor(Chart chart) : this(chart, new Dictionary<string, Series>())
        {
        }

        public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex)
        {
            _chart = chart;
            _seriesIndex = seriesIndex;

            Receive<InitializeChartMessage>(message => HandleInitialize(message));
            Receive<AddSeriesMessage>(message => HandleAddSeriesMessages(message));
        }

        #region Individual Message Type Handlers

        private void HandleAddSeriesMessages(AddSeriesMessage message)
        {
            var seriesName = message.Series.Name;
            if (!string.IsNullOrEmpty(seriesName) && !_seriesIndex.ContainsKey(seriesName))
            {
                _seriesIndex.Add(seriesName, message.Series);
                _chart.Series.Add(message.Series);
            }
        }

        private void HandleInitialize(InitializeChartMessage ic)
        {
            if (ic.InitialSeries != null)
            {
                //swap the two series out
                _seriesIndex = ic.InitialSeries;
            }

            //delete any existing series
            _chart.Series.Clear();

            //attempt to render the initial chart
            if (_seriesIndex.Any())
            {
                foreach (var series in _seriesIndex)
                {
                    //force both the chart and the internal index to use the same names
                    series.Value.Name = series.Key;
                    _chart.Series.Add(series.Value);
                }
            }
        }

        #endregion
    }
}