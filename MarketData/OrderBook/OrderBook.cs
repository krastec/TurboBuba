using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Exchanges;
using TurboBuba.Infrastructure;

namespace TurboBuba.MarketData.OrderBook
{    
    public class OrderBook
    {
        //private string _contract = String.Empty;
        //private int _priceScale = 0;
        private ContractInfo _contractInfo;

        private bool _snapshotApplied = false;
        private long _snapshotLastUpdateId = 0;
        private long _prevLastUpdateId = 0;
        private long _updatesCounter = 0;

        private List<OrderBookUpdate> _preSnapshotUpdates = new();

        private List<OrderBookData> _orderBooksHistory = new();

        public OrderBook(ContractInfo contractInfo)
        {
            _contractInfo = contractInfo;
        }

        public bool InitFromSnapshot()
        {
            return true;
        }

        public bool ApplyUpdate(OrderBookUpdate update)
        {
            if (!_snapshotApplied)
            {
                _preSnapshotUpdates.Add(update);
                Logger.Log("OrderBook: Pre-snapshot update stored. Total pre-snapshot updates: " + _preSnapshotUpdates.Count);
                return true;
            }

            return true;
        }
    }
}
