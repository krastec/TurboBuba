using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.DataFeeds
{
    public class ExchangeSubscriptionManager
    {
        public static string GetSubscriptionKey(string contract, ExchangeSubscription.SubscriptionType subscriptionType) 
        {
            if(subscriptionType == ExchangeSubscription.SubscriptionType.None)
            {
                throw new ArgumentException("Subscription type cannot be None", nameof(subscriptionType));
            }
            else if(subscriptionType == ExchangeSubscription.SubscriptionType.OrderBook)
            {
                return $"OB_{contract.ToUpper()}";
            }            
            
            throw new ArgumentOutOfRangeException(nameof(subscriptionType), "Unknown subscription type");            
        }

    }
}
