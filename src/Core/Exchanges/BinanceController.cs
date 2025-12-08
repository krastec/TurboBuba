using Binance.Net.Clients;
using Binance.Net.Interfaces;
using Binance.Net.SymbolOrderBooks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TurboBuba.Infrastructure;
using TurboBuba.MarketData.OrderBook;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace TurboBuba.Exchanges
{
    public class BinanceController : BaseExchangeController
    {
        private BinanceSocketClient _socketClient = null!;
        private BinanceRestClient _restClient = null!;

        public BinanceController() : base(ExchangesList.Binance)
        {
        }

        public override async Task Connect()
        {
            this.ConnectionStatusChanged(ExchangeConnectionStatus.Connecting);
            _restClient = new BinanceRestClient();            
            _socketClient = new BinanceSocketClient();

            var connection = await _socketClient.UsdFuturesApi.PrepareConnectionsAsync();                        
            this.ConnectionStatusChanged(ExchangeConnectionStatus.Connected);

            var sw = Stopwatch.StartNew();
            var time = await _restClient.SpotApi.ExchangeData.GetServerTimeAsync();
            sw.Stop();

            var serverTime = time.Data;
            var localTime = DateTime.UtcNow - TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds / 2);

            var TimeOffset = serverTime - localTime;
            Debug.WriteLine($"[{ExchangesList.Binance}] Time offset: {TimeOffset.TotalMilliseconds} ms");

        }

        public override async Task SubscribeOrderBook(string contract, int depth, IExchangeSubscriptionConsumer consumer)
        {            
            var contractInfo = this.GetContract(contract);
            if(contractInfo == null)
            {
                Logger.Error($"[{ExchangesList.Binance}] Cannot subscribe to order book for unknown contract '{contract}'");
                return;
            }
            //await GetOrderBookSnapshot(contractInfo);
            var subscriptionKey = ExchangeSubscriptionManager.GetSubscriptionKey(contractInfo, ExchangeSubscription.SubscriptionType.OrderBook);
            var existingSubscription = _subscriptionManager.GetSubscription(subscriptionKey);
            if(existingSubscription != null)
            {
                Logger.Debug($"[{ExchangesList.Binance}] Subscription to order book for contract '{contract}' already exists. Adding consumer to existing subscription.");
                _subscriptionManager.AddConsumerToSubscription(subscriptionKey, consumer);
                return;
            }
            else
            {
                string topic = $"{contract}@depth@100";
                var subscription = _subscriptionManager.AddSubscription(
                    subscriptionKey,
                    ExchangeSubscription.SubscriptionType.OrderBook,
                    contractInfo,
                    topic,
                    consumer,
                    new Dictionary<string, string> { { "depth", depth.ToString() } }
                );
                
                //await GetOrderBookSnapshot(contractInfo);

                OrderBook orderBook = _marketDataManager.OrderBooksManager.CreateOrGetOrderBook(contractInfo);                
                var stream = await this._socketClient.UsdFuturesApi.ExchangeData.SubscribeToOrderBookUpdatesAsync(contractInfo.Contract, 100, (data) => { OnOrderBookUpdate(data, orderBook); });
                subscription.SubscriptionId = stream.Data.Id;

                //_socketClient.UnsubscribeAsync(subscription.SubscriptionId);
                GetAndApplyOrderBookSnapshot(contractInfo);
            }
        }

        private async Task GetAndApplyOrderBookSnapshot(ContractInfo contractInfo)
        {
            var snapshot = await _restClient.UsdFuturesApi.ExchangeData.GetOrderBookAsync(contractInfo.Contract, 1000);
            var orderBook = _marketDataManager.OrderBooksManager.GetOrderBook(contractInfo);

            OrderBookUpdate update = new OrderBookUpdate();
            update.FirstUpdateId = 0;
            update.PrevLastUpdateId = 0;
            update.LastUpdateId = snapshot.Data.LastUpdateId;

            var bidsCount = snapshot.Data.Bids.Length;
            var asksCount = snapshot.Data.Asks.Length;

            update.Bids = bidsCount == 0 ? Array.Empty<scaledPrice[]>() : new scaledPrice[bidsCount][];
            update.Asks = asksCount == 0 ? Array.Empty<scaledPrice[]>() : new scaledPrice[asksCount][];

            for (int i = 0; i < bidsCount; i++)
            {
                var b = snapshot.Data.Bids[i];
                update.Bids[i] = new scaledPrice[2] { (scaledPrice)(b.Price * orderBook.PriceScale), (scaledPrice)(b.Quantity * orderBook.QtyScale) };
            }
            for (int i = 0; i < asksCount; i++)
            {
                var a = snapshot.Data.Asks[i];
                update.Asks[i] = new scaledPrice[2] { (scaledPrice)(a.Price * orderBook.PriceScale), (scaledPrice)(a.Quantity * orderBook.QtyScale) };
            }

            orderBook.InitFromSnapshot(update);
        }

        private void OnOrderBookUpdate(DataEvent<IBinanceFuturesEventOrderBook> data, OrderBook orderBook)
        {
            //data.ReceiveTime - data.Data.TransactionTime;
            var latency = data.ReceiveTime - data.Data.EventTime;
            var latencyMs = latency.TotalMilliseconds;
            Debug.WriteLine($"[{ExchangesList.Binance}] OB update latency: {latencyMs:F1} ms");

            //var serverEventTime = data.Data.EventTime;
            //var localReceiptTime = DateTime.UtcNow;
            //var latency = localReceiptTime - serverEventTime;
            //Debug.WriteLine($"WS latency: {latency.TotalMilliseconds} ms");

            OrderBookUpdate update = new OrderBookUpdate();
            update.FirstUpdateId = data.Data.FirstUpdateId;
            update.LastUpdateId = data.Data.LastUpdateId;
            update.PrevLastUpdateId = data.Data.LastUpdateIdStream;

            // Preallocate arrays for best performance
            var bidsCount = data.Data.Bids.Length;
            var asksCount = data.Data.Asks.Length;


            update.Bids = bidsCount == 0 ? Array.Empty<scaledPrice[]>() : new scaledPrice[bidsCount][];
            update.Asks = asksCount == 0 ? Array.Empty<scaledPrice[]>() : new scaledPrice[asksCount][];

            for (int i = 0; i < bidsCount; i++)
            {
                var b = data.Data.Bids[i];
                update.Bids[i] = new scaledPrice[2] { (scaledPrice)(b.Price * orderBook.PriceScale), (scaledPrice)(b.Quantity * orderBook.QtyScale) };
            }
            for (int i = 0; i < asksCount; i++)
            {
                var a = data.Data.Asks[i];
                update.Asks[i] = new scaledPrice[2] { (scaledPrice)(a.Price * orderBook.PriceScale), (scaledPrice)(a.Quantity * orderBook.QtyScale) };
            }

            var applied = orderBook.ApplyUpdate(update);
            if(applied == false)
            {
                orderBook.Reset();
                GetAndApplyOrderBookSnapshot(orderBook.ContractInfo);
            }

            //var contractInfo = GetContract("BTCUSDT");
            //var subscriptionKey = ExchangeSubscriptionManager.GetSubscriptionKey(contractInfo, ExchangeSubscription.SubscriptionType.OrderBook);
            //var existingSubscription = _subscriptionManager.GetSubscription(subscriptionKey);            
        }



    }
}
