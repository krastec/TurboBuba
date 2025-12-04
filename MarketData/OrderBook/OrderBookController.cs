using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.MarketData.OrderBook
{

    public class OrderBookController
    {
        private string _contract = String.Empty;
        private int _priceScale = 0;

        private bool _snapshotApplied = false;
        private long _snapshotLastUpdateId = 0;
        private long _prevLastUpdateId = 0;
        private long _updatesCounter = 0;

    }
}
