using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.MarketData.OrderBook;

namespace TurboBuba.MarketData
{
    public class MarketDataManager
    {
        private OrderBooksManager _orderBooksManager = new OrderBooksManager();

        public OrderBooksManager OrderBooksManager => _orderBooksManager;

    }
}
