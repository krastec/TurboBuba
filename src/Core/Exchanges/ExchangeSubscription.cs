using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TurboBuba.Exchanges
{

    public interface IExchangeSubscriptionConsumer { }

    public class ExchangeSubscription
    {
        public enum SubscriptionType
        {
            None,
            OrderBook
        }


        public string Key { get; } = String.Empty;
        public SubscriptionType Type { get; } = SubscriptionType.None;        
        public ContractInfo ContractInfo { get; } = null!;
        public string Topic { get; } = String.Empty;
        public List<IExchangeSubscriptionConsumer> Consumers { get; private set; }
        public Dictionary<string, string> ExtraData { get; } = [];
        public int SubscriptionId { get; set; } = -1;
        //public CancellationToken CancelationToken { get; set; } = default;

        public ExchangeSubscription(string key, SubscriptionType type, ContractInfo contractInfo, string topic, IExchangeSubscriptionConsumer consumer, Dictionary<string, string> extraData)
        {
            Key = key;
            Type = type;
            ContractInfo = contractInfo;
            Topic = topic;            
            Consumers = new List<IExchangeSubscriptionConsumer> { consumer };
            ExtraData = extraData;
        }

        public void AddConsumer(IExchangeSubscriptionConsumer consumer)
        {
            if (!Consumers.Contains(consumer))
            {
                Consumers.Add(consumer);
            }
        }

        public void RemoveConsumer(IExchangeSubscriptionConsumer consumer)
        {
            if (Consumers.Contains(consumer))
            {
                Consumers.Remove(consumer);
            }
        }
    }
}
