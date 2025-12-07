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

        private List<OrderBookData> _orderBooks = new();

        public int PriceScale => _contractInfo.PriceScale;
        public OrderBook(ContractInfo contractInfo)
        {
            _contractInfo = contractInfo;
        }

        public bool InitFromSnapshot(OrderBookUpdate update)
        {
            OrderBookData orderBook = new OrderBookData();
            orderBook.TimeStamp = TimeUtils.GetCurrentUnixTimestampMillis();
            //orderBook.Bids = new OrderBookLevel[update.Bids.Length];
            //orderBook.Asks = new OrderBookLevel[update.Asks.Length];
            //Array.Copy(update.Bids, orderBook.Bids, update.Bids.Length);
            //Array.Copy(update.Asks, orderBook.Asks, update.Asks.Length);

            _snapshotLastUpdateId += update.LastUpdateId;
            _snapshotApplied = true;
            _orderBooks.Add(orderBook);

            while (_preSnapshotUpdates.Count > 0 && this._preSnapshotUpdates[0].LastUpdateId <= _snapshotLastUpdateId)
            {
                _preSnapshotUpdates.RemoveAt(0);
            }
            if(this._preSnapshotUpdates.Count == 0)
            {
                Logger.Log("OrderBook: No pre-snapshot updates to apply.");
            }

            foreach (var preSnapshotUpdate in this._preSnapshotUpdates)
            {
                ApplyUpdate(preSnapshotUpdate);
            }

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

            var prevOrderBook = _orderBooks[_orderBooks.Count - 1];
            var newOrderBook = prevOrderBook.Clone();
            newOrderBook.TimeStamp = TimeUtils.GetCurrentUnixTimestampMillis();
            // Apply bids




            return true;
        }

        public void Reset()
        {
            _snapshotApplied = false;
            _snapshotLastUpdateId = 0;
            _prevLastUpdateId = 0;
            _updatesCounter = 0;
            _preSnapshotUpdates.Clear();
            _orderBooks.Clear();
        }
    }
}
