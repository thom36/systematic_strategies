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

    public class RegularRebalancing
    {
        public int Period { get; private set; }
        public RegularRebalancing(RegularOracleDescription regular)
        {
            Period = regular.Period;
        }

        public bool IsRebalancingEnabled(int time)
        {
            return time % Period == 0;
        }

    }

}