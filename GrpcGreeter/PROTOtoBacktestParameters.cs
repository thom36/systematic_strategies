using BacktestGrpc.Protos;
using Google.Protobuf.WellKnownTypes;
using MathNet.Numerics.Statistics;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.RebalancingOracleDescriptions;
public class PROTOtoBacktestParameters
{
    public PROTOtoBacktestParameters() { }

    public BasketTestParameters ConvertToBasketTestParam(TestParams testParams)
    {
        var pricingParams = LoadBasketPricingParameters(testParams.PriceParams);
        var basketOption = LoadBasket(testParams.BasketParams);
        var rebalancing = LoadRebalancing(testParams.RebParams);

        var basketTestParam = new BasketTestParameters()
        {
            PricingParams = pricingParams,
            BasketOption = basketOption,
            RebalancingOracleDescription = rebalancing
        };

        return basketTestParam;
    }

    private BasketPricingParameters LoadBasketPricingParameters(PricingParams pricingParams)
    {
        var basket = new BasketPricingParameters();
        for (int i = 0; i < pricingParams.Vols.Count; i++)
        {
            basket.Volatilities[i] = pricingParams.Vols[i];
        }

        for (int i = 0; i < pricingParams.Corrs.Count; i++)
        {
            for (int j = 0; j < pricingParams.Corrs[i].Value.Count; j++)
            {
                basket.Correlations[i][j] = pricingParams.Corrs[i].Value[j];
            }
        }
        return basket;
    }

    private Basket LoadBasket(BasketParams basketParams)
    {
        var basket = new Basket();
        basket.Strike = basketParams.Strike;

        basket.Maturity = basketParams.Maturity.ToDateTime();

        for (int i = 0; i < basketParams.ShareIds.Count; i++)
        {
            basket.UnderlyingShareIds[i] = basketParams.ShareIds[i];
        }

        for (int i = 0; i < basketParams.Weights.Count; i++)
        {
            basket.Weights[i] = basketParams.Weights[i];
        }
        return basket;
    }

    private IRebalancingOracleDescription LoadRebalancing(RebalancingParams reb)
    {
        var rebalancing = new RegularOracleDescription()
        {
            Period = reb.Regular.Period
        };

        return rebalancing;
    }
}