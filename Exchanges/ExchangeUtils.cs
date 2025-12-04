using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.DataFeeds
{
    public class ExchangeUtils
    {
        public static string GetExchangeName(Exchanges exchange)
        {
            return exchange switch
            {
                Exchanges.None => "None",
                Exchanges.Binance => "Binance",
                _ => "Unknown"
            };
        }
    }
}
