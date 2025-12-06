using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.MarketData.OrderBook
{
    public class OrderBookData
    {
        public long TimeStamp = 0;
        public SortedList<long, long> Bids = new();
        public SortedList<long, long> Asks = new();

        public OrderBookData Clone()
        {
            OrderBookData clone = new OrderBookData();
            clone.TimeStamp = this.TimeStamp;


            // Preserve comparer and copy entries(shallow copy of values)
            clone.Bids = new SortedList<long, long>(this.Bids.Comparer);
            foreach (var kv in this.Bids)
            {
                clone.Bids.Add(kv.Key, kv.Value);
            }

            clone.Asks = new SortedList<long, long>(this.Asks.Comparer);
            foreach (var kv in this.Asks)
            {
                clone.Asks.Add(kv.Key, kv.Value);
            }

            return clone;
        }
    }
    /*
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
    */
    public class OrderBookUpdate
    {
        public long? FirstUpdateId = 0;
        public long LastUpdateId = 0;
        public long PrevLastUpdateId = 0;
        public long[][] Bids = Array.Empty<long[]>();
        public long[][] Asks = Array.Empty<long[]>();
        //public OrderBookLevel[] Bids = Array.Empty<OrderBookLevel>();
        //public OrderBookLevel[] Asks = Array.Empty<OrderBookLevel>();
    }
}
