using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Infrastructure;

namespace TurboBuba.DataFeeds
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
        private readonly Exchanges _exchange = Exchanges.None;

        private Dictionary<string, ContractInfo> _contracts = new();

        public bool IsConnected { get; protected set; } = false;

        public BaseExchangeController(Exchanges exchange)
        {
            _exchange = exchange;
        }

        #region Abstract methods
        public abstract void Connect();
        public abstract void SubscribeOrderBook(string contract, int depth);
        #endregion

        #region Events
        protected void ConnectionStatusChanged(ExchangeConnectionStatus status)
        {
            AppController.Instance.EventBus.Publish(new ExchangeEvents.ConnectionStatusChanged(this, status));

            Logger.Log($"[{_exchange}] Connection status changed to {status}");
        }
        #endregion

        public ContractInfo RegisterContract(string contract, int priceScale, int multiplier)
        {
            {
                var upperContract = contract.ToUpperInvariant();
                if (!_contracts.ContainsKey(contract))
                {
                    _contracts.Add(contract, new ContractInfo(contract, priceScale, multiplier));
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

        
    }
}
