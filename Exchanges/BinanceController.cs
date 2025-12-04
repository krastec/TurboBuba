using Binance.Net.Clients;
using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Infrastructure;

namespace TurboBuba.Exchanges
{
    public class BinanceController : BaseExchangeController
    {
        private BinanceSocketClient _socketClient = null!;

        public BinanceController() : base(ExchangesList.Binance)
        {
        }

        public override async void Connect()
        {
            this.ConnectionStatusChanged(ExchangeConnectionStatus.Connecting);
            _socketClient = new BinanceSocketClient();
            await _socketClient.UsdFuturesApi.PrepareConnectionsAsync();            
            this.ConnectionStatusChanged(ExchangeConnectionStatus.Connected);
        }

        public override void SubscribeOrderBook(string contract, int depth, IExchangeSubscriptionConsumer consumer)
        {
            var contractInfo = this.GetContract(contract);
            if(contractInfo == null)
            {
                Logger.Error($"[{ExchangesList.Binance}] Cannot subscribe to order book for unknown contract '{contract}'");
                return;
            }

            var subscriptionKey = ExchangeSubscriptionManager.GetSubscriptionKey(contract, contractInfo.ContractType, ExchangeSubscription.SubscriptionType.OrderBook);
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
                    contractInfo.ContractType,
                    contract,
                    topic,
                    consumer,
                    new Dictionary<string, string> { { "depth", depth.ToString() } }
                );

                //this._socketClient.UsdFuturesApi.ExchangeData.SubscribeToOrderBookUpdatesAsync);
                //subscription.CancelationToken = ...;
            }

        }


    }
}
