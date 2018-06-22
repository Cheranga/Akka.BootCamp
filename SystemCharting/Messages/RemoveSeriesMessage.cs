namespace SystemCharting.Messages
{
    public class RemoveSeriesMessage
    {
        public RemoveSeriesMessage(string seriesName)
        {
            SeriesName = seriesName;
        }

        public string SeriesName { get; }
    }
}