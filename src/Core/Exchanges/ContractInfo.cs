using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.Exchanges
{
    public enum ContractType
    {
        Spot,
        Perp
    }

    public class ContractInfo
    {
        public string Contract { get;  } = string.Empty;
        public ContractType ContractType { get; } = ContractType.Perp;
        public int PriceScale { get;  } = 0;
        public int QtyScale { get; } = 0;
        public int Multiplier { get; } = 1;

        public ContractInfo(string contract, ContractType contractType, int priceScale, int qtyScale, int multiplier)
        {
            Contract = contract;
            ContractType = contractType;
            PriceScale = priceScale;
            QtyScale = qtyScale;
            Multiplier = multiplier;
        }
    }
}
