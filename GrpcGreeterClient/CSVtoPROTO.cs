using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using CsvHelper;
using Google.Protobuf.WellKnownTypes;

namespace BacktestGrpc.Protos
{
    public class CSVtoPROTO
    {
        public static DataParams LoadCsv(string filePath)
        {
            DataParams dataParams = new DataParams();
            using (var streamer = new StreamReader(filePath))
            using (var csv = new CsvReader(streamer, CultureInfo.InvariantCulture))
            {
               if(csv.Read())
                {
                    csv.ReadHeader();

                    while(csv.Read())
                    {
                        string date = csv.GetRecord<string>("DateOfPrice");
                        var dateOfPrice = DateTime.ParseExact(date, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        ShareData shareData = new ShareData()
                        {
                            Id = csv.GetField<string>("Id"),
                            Value = csv.GetField<double>("Value"),
                            Date = Timestamp.FromDateTime(dateOfPrice)
                        };
                        dataParams.DataValues.Add(shareData);
                    }
                }

            }
            return dataParams;
        }
    }
}
