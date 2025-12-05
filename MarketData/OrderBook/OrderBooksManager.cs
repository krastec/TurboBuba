
using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Exchanges;

namespace TurboBuba.MarketData.OrderBook
{
    public class OrderBooksManager
    {
        public Dictionary<string, OrderBook> _orderBooks = new();

        public OrderBook GetOrderBook(ContractInfo contractInfo)
        {
            string suffix = ExchangeUtils.GetSuffixByContractType(contractInfo.ContractType);
            string key = $"{contractInfo.Contract}_{suffix}";
            if (_orderBooks.ContainsKey(key))
            {
                return _orderBooks[key];
            }
            return null!;
        }

        public OrderBook CreateOrGetOrderBook(ContractInfo contractInfo)
        {
            string suffix = ExchangeUtils.GetSuffixByContractType(contractInfo.ContractType);
            string key = $"{contractInfo.Contract}_{suffix}";
            if (!_orderBooks.ContainsKey(key))
            {
                var orderBook = new OrderBook(contractInfo);
                _orderBooks[key] = orderBook;
                return orderBook;
            }
            return _orderBooks[key];
        }
    }
}
