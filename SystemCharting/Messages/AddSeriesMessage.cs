using System.Windows.Forms.DataVisualization.Charting;

namespace SystemCharting.Messages
{
    public class AddSeriesMessage
    {
        public Series Series { get; }

        public AddSeriesMessage(Series series)
        {
            Series = series;
        }
    }
}