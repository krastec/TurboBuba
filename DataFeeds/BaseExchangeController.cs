using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.DataFeeds
{
    public abstract class BaseExchangeController
    {
        private readonly ExchangesList _exchange = ExchangesList.None;

        private Dictionary<string, ContractInfo> _contracts = new();

        public BaseExchangeController(ExchangesList exchange)
        {
            _exchange = exchange;
        }

        #region Abstract methods
        public abstract void SubscribeOrderBook(string contract, int depth);
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
