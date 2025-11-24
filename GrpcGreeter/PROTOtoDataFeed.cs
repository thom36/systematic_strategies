using BacktestGrpc.Protos;
using PricingLibrary.MarketDataFeed;
public class PROTOtoDataFeed
{
  public static List<DataFeed> ConvertToDataFeed(DataParams dataParams)
    {
        List<DataFeed> dataFeeds = new List<DataFeed>();

        var shareData = dataParams.DataValues[0];
        var date = shareData.Date;
        var dict = new Dictionary<string, double>();
        dict.Add(shareData.Id, shareData.Value);

        for (int i = 1; i < dataParams.DataValues.Count; i++)
        {
            shareData = dataParams.DataValues[i];
            if(date ==  shareData.Date)
            {
                dict.Add(shareData.Id, shareData.Value);
            }
            else
            {
                var dataFeed = new DataFeed(date.ToDateTime(), dict);
                dataFeeds.Add(dataFeed);
                date = shareData.Date;
                dict.Clear();
                dict.Add(shareData.Id, shareData.Value);
            }
        }
        return dataFeeds;
    }
}