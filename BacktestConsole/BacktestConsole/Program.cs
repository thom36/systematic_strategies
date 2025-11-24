using System;
using System.Collections.Generic;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using Reader;
using System.Text.Json;
using Control;


public class Program
{
    public static void Main(string[] args)
    {
        /*string jsonFilePath = args[0];
        string csvFilePath = args[1];*/

        string jsonFilePath = @"C:\Users\localuser\Documents\GitHub\systematic_strategies\TestData\Test_1_1\params_1_1.json";
        string csvFilePath = @"C:\Users\localuser\Documents\GitHub\systematic_strategies\TestData\Test_1_1\data_1_1.csv";

        BasketTestParameters param = JsonReader.LoadJson(jsonFilePath);

        List<DataFeed> dataFeeds = CSVReader.LoadCsv(csvFilePath);

        var strategy = new Strategy(param);
        List<OutputData> output = strategy.Run(dataFeeds);

        //string fileName = args[2];
        string fileName = "output.json";
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
        string jsonString = JsonSerializer.Serialize(output, options);
        File.WriteAllText(fileName, jsonString);
    }
}
