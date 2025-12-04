using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Infrastructure;

namespace TurboBuba.Exchanges
{
    public class ExchangeSubscriptionManager
    {
        private Dictionary<string, ExchangeSubscription> _subscriptions = new Dictionary<string, ExchangeSubscription>();

        public static string GetSubscriptionKey(string contract, ContractType contractType, ExchangeSubscription.SubscriptionType subscriptionType) 
        {
            string keySuffix = contractType == ContractType.Spot ? "SPOT" : "PERP";

            if (subscriptionType == ExchangeSubscription.SubscriptionType.None)
            {
                throw new ArgumentException("Subscription type cannot be None", nameof(subscriptionType));
            }
            else if(subscriptionType == ExchangeSubscription.SubscriptionType.OrderBook)
            {
                return $"OB_{contract.ToUpper()}_${keySuffix}";
            }            
            
            throw new ArgumentOutOfRangeException(nameof(subscriptionType), "Unknown subscription type");            
        }

        public bool HasSubscription(string key)
        {
            return _subscriptions.ContainsKey(key);
        }

        public ExchangeSubscription AddSubscription(string key, ExchangeSubscription.SubscriptionType type, ContractType contractType, string contract, string topic, IExchangeSubscriptionConsumer consumer, Dictionary<string, string> extraData)
        {
            if (_subscriptions.ContainsKey(key))
            {
                Logger.Debug($"Subscription with key '{key}' already exists.");
                return _subscriptions[key];
            }
            var subscription = new ExchangeSubscription(key, type, contract, contractType, topic, consumer, extraData);
            _subscriptions[key] = subscription;
            return subscription;
        }

        public void RemoveSubscription(string key)
        {
            if (_subscriptions.ContainsKey(key))
            {
                _subscriptions.Remove(key);
            }
            else
            {
                Logger.Debug($"Subscription with key '{key}' does not exist.");
            }
        }

        public void AddConsumerToSubscription(string key, IExchangeSubscriptionConsumer consumer)
        {
            if (_subscriptions.ContainsKey(key))
            {
                _subscriptions[key].AddConsumer(consumer);
            }
        }

        public ExchangeSubscription? GetSubscription(string key)
        {
            if (_subscriptions.ContainsKey(key))
            {
                return _subscriptions[key];
            }
            return null;
        }


    }
}
