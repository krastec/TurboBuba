using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.OrderBook
{
    public class OrderBook
    {
        public long TimeStamp = 0;
        public long[][] Bids = new long[0][];
        public long[][] Asks = new long[0][];
    }
}
