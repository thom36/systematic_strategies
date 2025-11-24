using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using PricingLibrary.MarketDataFeed;
using CsvHelper;

namespace Reader
{

    public class CSVReader
    {
        public static List<DataFeed> LoadCsv(string csvFilePath)
        {
            using (var streamer = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(streamer, CultureInfo.InvariantCulture))
            {

                var shareValues = csv.GetRecords<ShareValue>();
                var dataFeeds = shareValues
                    .GroupBy(d => d.DateOfPrice,
                             t => new { Symb = t.Id.Trim(), Val = t.Value },
                             (key, g) => new DataFeed(key, g.ToDictionary(e => e.Symb, e => e.Val)))
                    .ToList();

                return dataFeeds;
            }
        }

    }

}

