using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Events;
using TurboBuba.Exchanges;
using TurboBuba.Infrastructure;

namespace TurboBuba.Signal
{
    /// <summary>
    /// Подписывается на нужные события в приложении и рассылает их клентам
    /// </summary>
    public class PilotEventsRouter : IDisposable
    {
        private AppController _appController;

        private EventSubscriber _subscriber;
        private PilotHub _hub;

        public PilotEventsRouter(AppController appController, PilotHub hub)
        {
            _appController = appController;
            _hub = hub;
        }
        

        public void Start()
        {
            _subscriber = new EventSubscriber(_appController.EventBus);

            this.SubscribeToExchangeEvents();
        }

        private void SubscribeToExchangeEvents()
        {
            _subscriber.Subscribe<ExchangeEvents.ConnectionStatusChanged>(OnExchangeConnectionStatusChanged, this);
        }
        private void OnExchangeConnectionStatusChanged(ExchangeEvents.ConnectionStatusChanged evt)
        {
            SignalModels.ExchangeStatus exchangeStatus = new SignalModels.ExchangeStatus(
                Exchange: evt.Exchange.ExchangeId,
                Connected: evt.Status == ExchangeConnectionStatus.Connected ? 1 : 0,
                Ping_in: 0,
                Ping_out: 0
            );

            _hub.Broadcast<SignalModels.ExchangeStatus>(SignalModels.Methods.ExchangeStatusChanged, exchangeStatus);
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }
    }
}
