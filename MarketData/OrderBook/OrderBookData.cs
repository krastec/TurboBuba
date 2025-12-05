using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.MarketData.OrderBook
{
    public class OrderBookData
    {
        public long TimeStamp = 0;
        public long[][] Bids = new long[0][];
        public long[][] Asks = new long[0][];
    }

    public struct OrderBookLevel
    {
        public decimal Price = 0;
        public decimal Quantity = 0;
        public OrderBookLevel(decimal price, decimal quantity)
        {
            Price = price;
            Quantity = quantity;
        }
    }

    public class OrderBookUpdate
    {
        public long? FirstUpdateId = 0;
        public long LastUpdateId = 0;
        public long PrevLastUpdateId = 0;
        public OrderBookLevel[] Bids = Array.Empty<OrderBookLevel>();
        public OrderBookLevel[] Asks = Array.Empty<OrderBookLevel>();
    }
}
