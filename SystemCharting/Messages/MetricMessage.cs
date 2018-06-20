using System.Windows.Forms.DataVisualization.Charting;

namespace SystemCharting.Messages
{
    public class MetricMessage
    {
        public string Series { get; }
        public float CounterValue { get; }

        public MetricMessage(string series, float counterValue)
        {
            Series = series;
            CounterValue = counterValue;
        }
    }
}