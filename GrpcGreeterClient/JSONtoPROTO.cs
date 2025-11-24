using System.Text.Json.Serialization;
using System.Text.Json;
using PricingLibrary.RebalancingOracleDescriptions;
using PricingLibrary.DataClasses;
using Google.Protobuf.WellKnownTypes;

namespace BacktestGrpc.Protos
{
    public class JSONtoPROTO
    {
        public JSONtoPROTO() { }
        public TestParams LoadTestParams(string filePath)
        {
            var basket = LoadJson(filePath);
            var pricingParams = LoadPricingParams(basket.PricingParams);
            var basketParams = LoadBasketParams(basket.BasketOption);
            var regularRebalancing = LoadRebalancingParams(basket.RebalancingOracleDescription);

            var testParams = new TestParams()
            {
                PriceParams = pricingParams,
                BasketParams = basketParams,
                RebParams = regularRebalancing
            };
            return testParams;
        }

        private static BasketTestParameters LoadJson(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(), new RebalancingOracleDescriptionConverter() }
            };

            string jsonString = File.ReadAllText(filePath);
            BasketTestParameters jsonData = JsonSerializer.Deserialize<BasketTestParameters>(jsonString, options);

            return jsonData;

        }

        private PricingParams LoadPricingParams(BasketPricingParameters basket)
        {
            var pricingParams = new PricingParams();
            for (int i = 0; i < basket.Volatilities.Length; i++)
            {
                pricingParams.Vols.Add(basket.Volatilities[i]);
            }

            var corrline = new CorrLine();
            for (int i = 0; i < basket.Correlations.Length; i++)
            {
                for(int j = 0; j < basket.Correlations[i].Length; j++)
                {
                    corrline.Value.Add(basket.Correlations[i][j]);
                }
                pricingParams.Corrs.Add(corrline);
            }
            return pricingParams;  
        }

        private BasketParams LoadBasketParams(Basket basket)
        {
            var basketParams = new BasketParams();
            basketParams.Strike = basket.Strike;

            basketParams.Maturity = Timestamp.FromDateTime(basket.Maturity);

            for(int i = 0; i < basket.UnderlyingShareIds.Length; i++)
            {
                basketParams.ShareIds.Add(basket.UnderlyingShareIds[i]);
            }

            for(int i = 0; i < basket.Weights.Length; i++)
            {
                basketParams.Weights.Add(basket.Weights[i]);
            }
            return basketParams;
        }

        private RebalancingParams LoadRebalancingParams(IRebalancingOracleDescription rebalancing)
        {
            var regularRebalancing = (RegularOracleDescription) rebalancing;
            var reg = new RegularRebalancing();
            reg.Period = regularRebalancing.Period;

            var rebalancingParams = new RebalancingParams()
            {
                Regular = reg
            };
            return rebalancingParams;
        }
    }
}