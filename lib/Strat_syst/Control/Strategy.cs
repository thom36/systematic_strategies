using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Model;
using PricingLibrary;
using PricingLibrary.Computations;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.RebalancingOracleDescriptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Control
{



    public class Strategy
    {
        public Portfolio Portfolio { get;  private set; }

        public Pricer Pricer { get; private set; }

        public ConvertMathFinance Converter { get; private set; }

        public RegularRebalancing RegularRebalancing { get; private set; }

        public Strategy(BasketTestParameters basket) 
        {
            Pricer = new Pricer(basket);
            Converter = new ConvertMathFinance(basket.BasketOption.UnderlyingShareIds);
            IRebalancingOracleDescription rebalancing = basket.RebalancingOracleDescription;
            RegularRebalancing = new RegularRebalancing((RegularOracleDescription) rebalancing);
        }

        public List<OutputData> Run(List<DataFeed> dataFeeds)
        {
            var dataFeed = dataFeeds[0];
            double[] spots = Converter.GetSpots(dataFeed);
            PricingResults res = Pricer.Price(dataFeed.Date, spots);
            
            var outPutDatas = new List<OutputData>();
            OutputData outPutData = genOutputData(res, dataFeed.Date, res.Price);
            outPutDatas.Add(outPutData);

            Dictionary<string, double> compo = Converter.GetCompoPortfolio(res.Deltas);

            Portfolio = new Portfolio(compo, dataFeed, outPutData.Value);

            for (int t = 1; t < dataFeeds.Count; t++)
            {
                if(RegularRebalancing.IsRebalancingEnabled(t))
                {
                    dataFeed = dataFeeds[t];
                    spots = Converter.GetSpots(dataFeed);
                    res = Pricer.Price(dataFeed.Date, spots);

                    OutputData outputData = genOutputData(res, dataFeed.Date, Portfolio.GetPortfolioValue(dataFeed));
                    outPutDatas.Add(outputData);

                    compo = Converter.GetCompoPortfolio(outputData.Deltas);

                    Portfolio.UpdatePortfolio(compo, dataFeed);
                }
            }
            return outPutDatas;
        }

        private OutputData genOutputData(PricingResults res, DateTime date, double portfoliotValue)
        {
            var outputData = new OutputData()
            {
                Price = res.Price,
                Date = date,
                PriceStdDev = res.PriceStdDev,
                Deltas = res.Deltas,
                DeltasStdDev = res.DeltaStdDev,
                Value = portfoliotValue
            };
            return outputData;
        }
        }

    }