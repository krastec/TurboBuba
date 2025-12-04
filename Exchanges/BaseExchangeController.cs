using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Infrastructure;

namespace TurboBuba.Exchanges
{
    public enum ExchangeConnectionStatus
    {
        Unknown = 0,
        Connected = 1,
        Disconnected = 2,
        Connecting = 3
    }
    

    public abstract class BaseExchangeController
    {
        private readonly ExchangesList _exchange = ExchangesList.None;

        private Dictionary<string, ContractInfo> _contracts = new();
        protected ExchangeSubscriptionManager _subscriptionManager  = new();

        public bool IsConnected { get; protected set; } = false;

        public BaseExchangeController(ExchangesList exchange)
        {
            _exchange = exchange;
        }

        #region Abstract methods
        public abstract void Connect();
        public abstract void SubscribeOrderBook(string contract, int depth, IExchangeSubscriptionConsumer consumer);
        #endregion

        #region Events
        protected void ConnectionStatusChanged(ExchangeConnectionStatus status)
        {
            AppController.Instance.EventBus.Publish(new ExchangeEvents.ConnectionStatusChanged(this, status));

            Logger.Log($"[{_exchange}] Connection status changed to {status}");
        }
        #endregion

        #region Contract management
        public ContractInfo RegisterContract(string contract, ContractType type, int priceScale, int multiplier)
        {
            {
                var upperContract = contract.ToUpperInvariant();
                if (!_contracts.ContainsKey(contract))
                {
                    _contracts.Add(contract, new ContractInfo(contract, type, priceScale, multiplier));
                }

                return _contracts[contract];
            }
        }

        public ContractInfo? GetContract(string contract)
        {
            var upperContract = contract.ToUpperInvariant();
            if (_contracts.ContainsKey(upperContract))
            {
                return _contracts[upperContract];
            }
            return null;
        }
        #endregion

    }
}
