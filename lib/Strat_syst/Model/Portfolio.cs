using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PricingLibrary;
using PricingLibrary.Computations;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Model
{



    public class Portfolio
    {
        public Dictionary<string, double> Compo { get; private set; }
        public double WithoutRiskPart { get; private set; }
        public DateTime RebalancingDate { get; private set; }
        public Portfolio(Dictionary<string, double> compo, DataFeed dataFeed, double value)
        {
            Compo = compo;
            WithoutRiskPart = value - CalculateRiskPart(dataFeed.PriceList, compo);
            RebalancingDate = dataFeed.Date;
        }

        public double GetPortfolioValue(DataFeed dataFeed)
        {
            double withoutRiskPart = WithoutRiskPart * RiskFreeRateProvider.GetRiskFreeRateAccruedValue(RebalancingDate, dataFeed.Date);

            double riskPart = CalculateRiskPart(dataFeed.PriceList, Compo);

            double portfolioValue = riskPart + withoutRiskPart;

            return portfolioValue;
        }

        public void UpdatePortfolio(Dictionary<string, double> compo, DataFeed dataFeed)
        {
            double portfolioValue = GetPortfolioValue(dataFeed);
            Compo = compo;
            WithoutRiskPart = portfolioValue - CalculateRiskPart(dataFeed.PriceList, Compo);
            RebalancingDate = dataFeed.Date;
        }


        // To calculate a sum 
        private double CalculateRiskPart(Dictionary<string, double> underlyingSpots, Dictionary<string, double> compo)
        {
            double sum = 0;
            foreach (var c in compo)
            {
                string key = c.Key;
                double underlyingDelta = c.Value;
                double underlyingValue;

                if(underlyingSpots.TryGetValue(key, out underlyingValue)){
                    sum += (underlyingDelta * underlyingValue);
                }
            }
            return sum;
        }
    }

}