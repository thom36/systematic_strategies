using BacktestGrpc.Protos;
using PricingLibrary.DataClasses;

public class BacktestOutputToOutputData
{
    public BacktestOutputToOutputData() { }
    public  List<OutputData> ConvertToOutputData(BacktestOutput backtest)
    {
        var outputDatas = new List<OutputData>();
        var outputData = new OutputData();

        for(int i = 0; i < backtest.BacktestInfo.Count; i++)
        {
            outputData.Date = backtest.BacktestInfo[i].Date.ToDateTime();
            outputData.Value = backtest.BacktestInfo[i].PortfolioValue;
            LoadDeltas(backtest.BacktestInfo[i], outputData);
            LoadDeltasStDev(backtest.BacktestInfo[i], outputData);
            outputData.Price = backtest.BacktestInfo[i].Price;
            outputData.PriceStdDev = backtest.BacktestInfo[i].PriceStddev;
            
            outputDatas.Add(outputData);
        }


        return outputDatas;
    }

    private void LoadDeltas(BacktestInfo backtestInfo, OutputData outputData)
    {
        for (int i = 0; i < backtestInfo.Delta.Count; i++)
        {
            outputData.Deltas[i] = backtestInfo.Delta[i];
        }
    }

    private void LoadDeltasStDev(BacktestInfo backtestInfo, OutputData outputData)
    {
        for (int i = 0; i < backtestInfo.DeltaStddev.Count; i++)
        {
            outputData.DeltasStdDev[i] = backtestInfo.DeltaStddev[i];
        }
    }
}
