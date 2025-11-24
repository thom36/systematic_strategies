using System.Text.Json;
using System.Threading.Tasks;
using BacktestGrpc.Protos;
using Grpc.Net.Client;
using PricingLibrary.DataClasses;

public class Program
{
    public static async Task Main(string[] args)
    {
        // The port number must match the port of the gRPC server.
        using var channel = GrpcChannel.ForAddress("https://localhost:7177");

        string jsonFilePath = @"C:\Users\localuser\Documents\GitHub\systematic_strategies\TestData\Test_1_1\params_1_1.json";
        string csvFilePath = @"C:\Users\localuser\Documents\GitHub\systematic_strategies\TestData\Test_1_1\data_1_1.csv";

        var dataParam = CSVtoPROTO.LoadCsv(csvFilePath);
        var jsonToproto = new JSONtoPROTO();
        var testParams = jsonToproto.LoadTestParams(jsonFilePath);

        var backTestRequest = new BacktestRequest()
        {
            TstParams = testParams,
            Data = dataParam
        };

        var client = new BacktestRunner.BacktestRunnerClient(channel);
        var reply = await client.RunBacktestAsync(backTestRequest);

        var converter = new BacktestOutputToOutputData();
        List<OutputData> outputDatas = converter.ConvertToOutputData(reply);

        string fileName = "output.json";
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
        string jsonString = JsonSerializer.Serialize(outputDatas, options);
        File.WriteAllText(fileName, jsonString);

    }
    
}








