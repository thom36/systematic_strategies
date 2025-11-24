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



    public class ConvertMathFinance
    {
        public string[] UnderlyingNames { get; private set; }
        public ConvertMathFinance(string[] underlyingNames)
        {
            UnderlyingNames = underlyingNames;
        }

        public Dictionary<string, double> GetCompoPortfolio(double[] deltas)
        {

            var newCompo = new Dictionary<string, double>();

            for (int i = 0; i < UnderlyingNames.Length; i++)
            {
                newCompo.Add(UnderlyingNames[i], deltas[i]);
            }

            return newCompo;
        }

       public double[] GetSpots(DataFeed dataFeed)
        {
            double[] spots = new double[UnderlyingNames.Length];
            for (int i = 0; i < UnderlyingNames.Length; i++)
            {
                Dictionary<string, double> dict = dataFeed.PriceList;
                dict.TryGetValue(UnderlyingNames[i], out spots[i]);
            }
            return spots;

        }

    }

}