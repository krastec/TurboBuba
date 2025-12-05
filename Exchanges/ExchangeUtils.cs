using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.Exchanges
{
    public class ExchangeUtils
    {
        public static string GetExchangeName(ExchangesList exchange)
        {
            return exchange switch
            {
                ExchangesList.None => "None",
                ExchangesList.Binance => "Binance",
                _ => "Unknown"
            };
        }

        public static string GetSuffixByContractType(ContractType contractType)
        {
            return contractType switch
            {
                ContractType.Perp => "PERP",
                ContractType.Spot => "SPOT",
                _ => "UNKNOWN"
            };
        }
    }
}
