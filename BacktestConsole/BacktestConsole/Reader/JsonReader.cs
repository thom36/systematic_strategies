using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using PricingLibrary.DataClasses;
using PricingLibrary.RebalancingOracleDescriptions;

namespace Reader
{
    public class JsonReader
    {
        public static BasketTestParameters LoadJson(string jsonFilePath)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(), new RebalancingOracleDescriptionConverter() }
            };

            string jsonString = File.ReadAllText(jsonFilePath);
            BasketTestParameters jsonData = JsonSerializer.Deserialize<BasketTestParameters>(jsonString, options);

            return jsonData;
        }
    }

}