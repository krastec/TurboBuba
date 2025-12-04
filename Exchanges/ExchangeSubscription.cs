using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.DataFeeds
{

    public interface IExchangeSubscriptionConsumer {}

    public class ExchangeSubscription
    {
        public enum SubscriptionType
        {
            None,
            OrderBook
        }

        public string Key { get; } = String.Empty;
        public SubscriptionType Type { get; } = SubscriptionType.None;
        public string Contract { get; } = String.Empty;
        public string Topic { get; } = String.Empty;
        public IExchangeSubscriptionConsumer[] Consumers { get; } = [];
        public Dictionary<string, string> ExtraData { get; } = [];
    }
}
