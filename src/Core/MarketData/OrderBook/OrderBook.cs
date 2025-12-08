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

        private long _initialTimeStamp = 0;
        private int _skippedUpdatesCounter = 0;

        

        private List<OrderBookUpdate> _preSnapshotUpdates = new();
        private List<OrderBookData> _orderBooks = new();

        public int PriceScale => _contractInfo.PriceScale;
        public int QtyScale => _contractInfo.QtyScale;
        public int SkippedUpdatesCounter => _skippedUpdatesCounter;
        public ContractInfo ContractInfo => _contractInfo;

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

            //orderBook.Bids = new SortedList<decimal, decimal>();
            //orderBook.Asks = new SortedList<decimal, decimal>();
            orderBook.Bids = new SortedList<scaledPrice, scaledPrice>(Comparer<scaledPrice>.Create((x, y) => y.CompareTo(x)));
            orderBook.Asks = new SortedList<scaledPrice, scaledPrice>(Comparer<scaledPrice>.Create((x, y) => x.CompareTo(y)));

            // BIDS
            if (update.Bids != null)
            {
                foreach (var bid in update.Bids)
                {
                    if (bid == null || bid.Length < 2)
                        continue;
                    scaledPrice price = bid[0];
                    scaledPrice qty = bid[1];
                    if (qty != 0)
                    {
                        orderBook.Bids[price] = qty;
                    }
                }
            }
            // ASKS
            if (update.Asks != null)
            {
                foreach (var ask in update.Asks)
                {
                    if (ask == null || ask.Length < 2)
                        continue;
                    scaledPrice price = ask[0];
                    scaledPrice qty = ask[1];
                    if (qty != 0)
                    {
                        orderBook.Asks[price] = qty;
                    }
                }
            }


            _snapshotLastUpdateId = update.LastUpdateId;
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
                ApplyUpdate(preSnapshotUpdate, true);
            }

            _initialTimeStamp = TimeUtils.GetCurrentUnixTimestampMillis();

            return true;
        }

        public bool ApplyUpdate(OrderBookUpdate update, bool applyWithoutCopy = false)
        {
            if (!_snapshotApplied)
            {
                _preSnapshotUpdates.Add(update);
                Logger.Log("OrderBook: Pre-snapshot update stored. Total pre-snapshot updates: " + _preSnapshotUpdates.Count);
                return true;
            }

            if(_updatesCounter > 0 && update.PrevLastUpdateId != _prevLastUpdateId)
            {
                Logger.Warn($"OrderBook: Update sequence mismatch. Expected PrevLastUpdateId={_prevLastUpdateId}, but got {update.PrevLastUpdateId}. Update skipped.");
                _skippedUpdatesCounter++;
                return false;
            }

            var prevOrderBook = _orderBooks[_orderBooks.Count - 1];
            OrderBookData newOrderBook;
            if(applyWithoutCopy)
                newOrderBook = prevOrderBook;
            else
                newOrderBook = prevOrderBook.Clone();
            newOrderBook.TimeStamp = TimeUtils.GetCurrentUnixTimestampMillis();

            // BIDS: update levels (update.Bids is long[][] where [0]=priceScaled, [1]=qtyScaled)
            if (update.Bids != null)
            {
                foreach (var bid in update.Bids)
                {
                    if (bid == null || bid.Length < 2)
                        continue;

                    scaledPrice price = bid[0];
                    scaledPrice qty = bid[1];

                    if (qty == 0M)
                    {
                        newOrderBook.Bids.Remove(price);
                    }
                    else
                    {
                        // upsert price -> qty
                        newOrderBook.Bids[price] = qty;
                    }
                }
            }

            // ASKS: update levels (update.Asks is long[][] where [0]=priceScaled, [1]=qtyScaled)
            if (update.Asks != null)
            {
                foreach (var ask in update.Asks)
                {
                    if (ask == null || ask.Length < 2)
                        continue;

                    scaledPrice price = ask[0];
                    scaledPrice qty = ask[1];

                    if (qty == 0M)
                    {
                        newOrderBook.Asks.Remove(price);
                    }
                    else
                    {
                        // upsert price -> qty
                        newOrderBook.Asks[price] = qty;
                    }
                }
            }

            // bookkeeping
            if(applyWithoutCopy == false)
                _orderBooks.Add(newOrderBook);
            _updatesCounter++;
            _prevLastUpdateId = update.LastUpdateId;
            _skippedUpdatesCounter = 0;

            return true;
        }

        public void Reset()
        {
            _snapshotApplied = false;
            _snapshotLastUpdateId = 0;
            _prevLastUpdateId = 0;
            _updatesCounter = 0;
            _skippedUpdatesCounter = 0;
            _initialTimeStamp = 0;
            _preSnapshotUpdates.Clear();
            _orderBooks.Clear();
        }
    }
}
