using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.DataFeeds
{
    public class ContractInfo
    {
        public string Contract { get;  } = string.Empty;
        public int PriceScale { get;  } = 0;
        public int Multiplier { get; } = 1;

        public ContractInfo(string contract, int priceScale, int multiplier)
        {
            Contract = contract;
            PriceScale = priceScale;
            Multiplier = multiplier;
        }
    }
}
