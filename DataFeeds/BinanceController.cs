using Binance.Net.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.DataFeeds
{
    public class BinanceController : BaseExchangeController
    {
        private BinanceSocketClient _socketClient = null!;

        public BinanceController() : base(Exchanges.Binance)
        {
        }

        public override async void Connect()
        {
            this.ConnectionStatusChanged(ExchangeConnectionStatus.Connecting);
            _socketClient = new BinanceSocketClient();
            await _socketClient.UsdFuturesApi.PrepareConnectionsAsync();

            this.ConnectionStatusChanged(ExchangeConnectionStatus.Connected);
        }

        public override void SubscribeOrderBook(string contract, int depth)
        {
            throw new NotImplementedException();
        }


    }
}
