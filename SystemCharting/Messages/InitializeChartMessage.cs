using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace SystemCharting.Messages
{
    public class InitializeChartMessage
    {
        public InitializeChartMessage(Dictionary<string, Series> initialSeries)
        {
            InitialSeries = initialSeries;
        }

        public Dictionary<string, Series> InitialSeries { get; }
    }
}